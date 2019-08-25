using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations
{
    class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", ProductContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Ignore(b => b.DomainEvents);
            
            builder.OwnsOne(o => o.Dimensions);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.HasIndex(x => x.BarCode).IsUnique();
            
            builder.Property(post => post.CreatedAt)
                .HasField("_createdAt");

            builder.Property(post => post.CreatedBy)
                .HasField("_createdBy");

            builder.Property(post => post.LastUpdatedAt)
                .HasField("_lastUpdatedAt");

            builder.Property(post => post.LastUpdatedBy)
                .HasField("_lastUpdatedBy");
            
            builder.HasMany(x=>x.Pictures)
                .WithOne()
                .IsRequired(false);
            
            builder.HasOne(x=>x.Category)
                .WithMany()
                .HasForeignKey(x=>x.CategoryId)
                .IsRequired();

            builder.HasOne(o => o.ProductType)
                .WithMany()
                .HasForeignKey(x => x.ProductTypeId)
                .IsRequired();
        }
    }
}
