using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.EntityConfigurations.Category;
using U.ProductService.Persistance.EntityConfigurations.Manufacturer;
using U.ProductService.Persistance.EntityConfigurations.Picture;
using U.ProductService.Persistance.EntityConfigurations.Product;
using U.ProductService.Persistance.Extensions;

namespace U.ProductService.Persistance.Contexts
{
    public class ProductContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "Products";

        //db sets
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        //fields
        private readonly IMediator _mediator;

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            try
            {
                _mediator = this.GetService<IMediator>();
            }
            catch
            {
                _mediator = new NoMediator();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MimeTypeEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ManufacturerEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);

            ChangeTracker.DetectChanges();
            OnBeforeSaving();
            await this.BulkSaveChangesAsync(cancellationToken);

            return true;
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable)
                {
                    var now = DateTime.UtcNow;
                    var user = GetCurrentUser();
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues["LastUpdatedAt"] = now;
                            entry.CurrentValues["LastUpdatedBy"] = user;
                            break;

                        case EntityState.Added:
                            entry.CurrentValues["CreatedAt"] = now;
                            entry.CurrentValues["CreatedBy"] = user;
                            entry.CurrentValues["LastUpdatedAt"] = now;
                            entry.CurrentValues["LastUpdatedBy"] = user;
                            break;
                    }
                }
            }
        }

        private string GetCurrentUser()
        {
            return "todoUser"; // TODO implement your own logic

            // If you are using ASP.NET Core, you should look at this answer on StackOverflow
            // https://stackoverflow.com/a/48554738/2996339
        }
    }

    public class NoMediator : IMediator
    {
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
            return default;
        }

        public async Task<object> Send(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
            return default;
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
        {
            await Task.CompletedTask;
        }
    }
}
