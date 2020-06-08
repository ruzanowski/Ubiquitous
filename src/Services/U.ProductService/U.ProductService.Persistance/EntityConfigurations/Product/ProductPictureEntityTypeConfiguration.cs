using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Product
{
    public class ProductPictureEntityTypeConfiguration : IEntityTypeConfiguration<ProductPicture>
    {
        public void Configure(EntityTypeBuilder<ProductPicture> builder)
        {
            builder.ToTable("Product_Pictures", ProductContext.DEFAULT_SCHEMA);

            builder
                .HasKey(el => el.ProductPictureId);

            builder
                .Property(e => e.ProductPictureId)
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.ProductId)
                .IsRequired();

            builder
                .Property(e => e.PictureId)
                .IsRequired();

            builder.HasOne(e => e.Picture)
                .WithMany()
                .HasForeignKey(e => e.PictureId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Product)
                .WithMany(x=>x.Pictures)
                .HasForeignKey(e => e.ProductId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}