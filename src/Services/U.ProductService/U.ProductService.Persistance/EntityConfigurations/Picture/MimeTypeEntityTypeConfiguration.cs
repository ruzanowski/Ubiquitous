using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain.Aggregates.Picture;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Picture
{
    class MimeTypeEntityTypeConfiguration : IEntityTypeConfiguration<MimeType>
    {
        public void Configure(EntityTypeBuilder<MimeType> cardTypesConfiguration)
        {
            cardTypesConfiguration.ToTable("Pictures_MimeTypes", ProductContext.DEFAULT_SCHEMA);

            cardTypesConfiguration.HasKey(ct => ct.Id);

            cardTypesConfiguration.Property(ct => ct.Id)
                .ValueGeneratedNever()
                .IsRequired();

            cardTypesConfiguration.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
