using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using SmartStore.Persistance.EntityBuilders.Catalog;

namespace SmartStore.Persistance.Context
{
    public class SmartStoreContext : DbContext
    {
        public SmartStoreContext(DbContextOptions<SmartStoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityBuilder());
            modelBuilder.ApplyConfiguration(new ProductEntityBuilder());
            modelBuilder.ApplyConfiguration(new ManufacturerEntityBuilder());
        }
    }
}