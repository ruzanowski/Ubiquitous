using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Product
{
    class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Category.Category>
    {
        public void Configure(EntityTypeBuilder<Domain.Aggregates.Category.Category> builder)
        {
            builder.ToTable("Products_Categories", ProductContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Ignore(b => b.DomainEvents);
            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
