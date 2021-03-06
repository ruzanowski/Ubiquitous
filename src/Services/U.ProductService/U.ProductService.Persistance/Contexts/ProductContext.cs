﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Picture;
using U.ProductService.Domain.Entities.Product;
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
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ManufacturerPicture> ManufacturerPictures { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<MimeType> MimeTypes { get; set; }
        //fields

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new MimeTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductPictureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ManufacturerPictureEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ManufacturerEntityTypeConfiguration());
        }

        public void StoreDomainEvents(IDomainEventsService domainEventsService, IMediator mediator, CancellationToken cancellationToken = default)
        {
            var domainEvents = this.GetDomainEvents();

            domainEventsService.AddDomainEvents(domainEvents);
        }

        public async Task<bool> SaveEntitiesAsync(IDomainEventsService domainEventsService, IMediator mediator, CancellationToken cancellationToken = default)
        {
            StoreDomainEvents(domainEventsService, mediator, cancellationToken);

            OnBeforeSaving();
            await this.BulkSaveChangesAsync(cancellationToken);
            await mediator.DispatchDomainEventsAsync(domainEventsService);
            return true;
        }

        private void OnBeforeSaving()
        {
            ChangeTracker.DetectChanges();

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
}
