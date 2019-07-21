using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            const int precision = 18;
            const int scale = 4;

            builder.ToTable("Product").HasKey(a => a.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(400);
            builder.Property(x => x.MetaKeywords).HasMaxLength(400);
            builder.Property(x => x.MetaTitle).HasMaxLength(400);
            builder.Property(x => x.FullDescription);


            builder.Property(x => x.Sku).HasMaxLength(400);
            builder.Property(x => x.ManufacturerPartNumber).HasMaxLength(400);
            builder.Property(x => x.AdditionalShippingCharge);
            builder.Property(x => x.Price).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.OldPrice).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.ProductCost).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.SpecialPrice).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.MinimumCustomerEnteredPrice).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.Weight).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.Length).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.Width).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.Height).HasColumnType($"decimal({precision},{scale})");
            builder.Property(x => x.AllowedQuantities).HasMaxLength(1000);
            builder.Property(x => x.SystemName).HasMaxLength(400);
            builder.Ignore(p => p.ProductType);
            builder.Ignore(p => p.ProductTypeLabelHint);
            builder.Ignore(p => p.MergedDataIgnore);
            builder.Ignore(p => p.MergedDataValues);
            builder.Ignore(x => x.BasePriceHasValue);
        }
    }
}