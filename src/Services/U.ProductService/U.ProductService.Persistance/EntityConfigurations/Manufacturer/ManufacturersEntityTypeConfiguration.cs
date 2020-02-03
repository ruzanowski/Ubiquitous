using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Manufacturer
{
    class ManufacturerEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Manufacturer.Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Domain.Aggregates.Manufacturer.Manufacturer> builder)
        {
            builder.ToTable("Manufacturers", ProductContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Ignore(b => b.DomainEvents);
            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            
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
        }
    }
}
