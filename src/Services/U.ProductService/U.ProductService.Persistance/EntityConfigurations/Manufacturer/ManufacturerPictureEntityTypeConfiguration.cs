using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Picture;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Manufacturer
{
    public class ManufacturerPictureEntityTypeConfiguration : IEntityTypeConfiguration<ManufacturerPicture>
    {
        public void Configure(EntityTypeBuilder<ManufacturerPicture> builder)
        {
            builder.ToTable("Manufacturer_Pictures", ProductContext.DEFAULT_SCHEMA);

            builder
                .HasKey(el => el.ManufacturerPictureId);

            builder
                .Property(e => e.ManufacturerPictureId)
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.ManufacturerId)
                .IsRequired();

            builder
                .Property(e => e.PictureId)
                .IsRequired();

            builder.HasOne(e => e.Picture)
                .WithMany()
                .HasForeignKey(e => e.PictureId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Manufacturer)
                .WithMany(x=>x.Pictures)
                .HasForeignKey(e => e.ManufacturerId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}