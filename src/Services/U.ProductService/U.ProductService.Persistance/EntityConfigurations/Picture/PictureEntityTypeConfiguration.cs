using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Picture
{
    public class PictureEntityTypeConfiguration: IEntityTypeConfiguration<Domain.Picture>
    {
        public void Configure(EntityTypeBuilder<Domain.Picture> builder)
        {
            builder.ToTable("Pictures", ProductContext.DEFAULT_SCHEMA);
            
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Ignore(b => b.DomainEvents);
            
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.Url).IsRequired();
        }
    }
}