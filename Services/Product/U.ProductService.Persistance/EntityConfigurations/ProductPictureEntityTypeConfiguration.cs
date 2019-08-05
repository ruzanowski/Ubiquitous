using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain.Aggregates;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations
{
    public class ProductPictureEntityTypeConfiguration: IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable("Products_Pictures", ProductContext.DEFAULT_SCHEMA);
            
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Ignore(b => b.DomainEvents);
            
            builder.Property(x => x.SeoFilename).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.SeoFilename).IsRequired();
            builder.Property(x => x.Url).IsRequired();
        }
    }
}