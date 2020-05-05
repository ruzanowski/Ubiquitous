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
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new ManufacturerMap());
            modelBuilder.ApplyConfiguration(new ProductManufacturerMap());
            modelBuilder.ApplyConfiguration(new ProductCategoryMap());
        }
    }
}