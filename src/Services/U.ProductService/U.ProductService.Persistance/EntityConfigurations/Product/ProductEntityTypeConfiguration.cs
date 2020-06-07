using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Product
{
    class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Product.Product>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Product.Product> builder)
        {
            builder.ToTable("Products", ProductContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Ignore(b => b.DomainEvents);

            builder.OwnsOne(o => o.Dimensions,
                d =>
                {
                    d.WithOwner();
                });

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.HasIndex(x => new {x.ExternalId, x.ExternalSourceName}).IsUnique();


            builder.Property(post => post.CreatedAt)
                .HasField("_createdAt");

            builder.Property(post => post.CreatedBy)
                .HasField("_createdBy");

            builder.Property(post => post.LastUpdatedAt)
                .HasField("_lastUpdatedAt");

            builder.Property(post => post.LastUpdatedBy)
                .HasField("_lastUpdatedBy");

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
