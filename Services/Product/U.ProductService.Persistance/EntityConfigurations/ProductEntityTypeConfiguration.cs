using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain.Aggregates;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations
{
    class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products", ProductContext.DEFAULT_SCHEMA);
            

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Ignore(b => b.DomainEvents);
            
            builder.OwnsOne(o => o.Dimensions);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            
            builder.HasAlternateKey(x => x.BarCode);
            builder.Property(x => x.BarCode).IsRequired();
            
            builder.Property(x => x.CreatedDateTime).IsRequired();
            
            builder.HasMany(x=>x.Pictures)
                .WithOne()
                .IsRequired(false);
        }
    }
}
