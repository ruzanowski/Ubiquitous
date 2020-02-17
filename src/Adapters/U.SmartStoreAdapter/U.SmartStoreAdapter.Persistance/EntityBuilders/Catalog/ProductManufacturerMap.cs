using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
{
    
    public class ProductManufacturerMap : IEntityTypeConfiguration<ProductManufacturer>
    {
        public void Configure(EntityTypeBuilder<ProductManufacturer> builder)
        {

            builder.ToTable("Product_Manufacturer_Mapping")
                .HasKey(x => x.Id);    
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
                
            builder.HasOne(p => p.Product) 
                .WithMany(x=>x.ProductManufacturers)
                .HasForeignKey(x => x.ProductId)
                .IsRequired();
            
            builder.HasOne(p => p.Manufacturer) 
                .WithMany()
                .HasForeignKey(x => x.ManufacturerId)
                .IsRequired();

        }
    }
}