using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.EntityConfigurations.Picture
{
    public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Picture.Picture>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Picture.Picture> builder)
        {
            builder.ToTable("Pictures", ProductContext.DEFAULT_SCHEMA);

            builder.HasKey(c => c.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FileStorageUploadId).ValueGeneratedNever();
            builder.Property(x => x.MimeTypeId).ValueGeneratedNever();
            builder.Ignore(b => b.DomainEvents);

            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.Url).IsRequired();

            builder.HasOne(o => o.MimeType)
                .WithMany()
                .HasForeignKey(x=>x.MimeTypeId)
                .IsRequired();
        }
    }
}