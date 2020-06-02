using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Picture
{
    public class MimeTypeEntityTypeConfiguration : IEntityTypeConfiguration<MimeType>
    {
        public void Configure(EntityTypeBuilder<MimeType> builder)
        {
            builder.ToTable("Pictures_MimeTypes", ProductContext.DEFAULT_SCHEMA);

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
