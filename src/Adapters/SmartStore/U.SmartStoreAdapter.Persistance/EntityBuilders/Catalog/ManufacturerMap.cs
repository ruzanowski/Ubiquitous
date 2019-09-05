using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
{
	
    public class ManufacturerMap : IEntityTypeConfiguration<Manufacturer>
    {
	    public void Configure(EntityTypeBuilder<Manufacturer> builder)
	    {
		    builder.ToTable("Manufacturer").HasKey(a => a.Id);
		    builder.Property(x => x.Id).IsRequired();
		    builder.Property(m => m.Name).IsRequired().HasMaxLength(400);
		    builder.Property(m => m.Description);
		    builder.Property(m => m.MetaKeywords).HasMaxLength(400);
		    builder.Property(m => m.MetaTitle).HasMaxLength(400);
		    builder.Property(m => m.PageSizeOptions).HasMaxLength(200).IsRequired(false);

		    builder.HasOne(p => p.Picture)
			    .WithMany(x=>x.Manufacturers)
			    .HasForeignKey(p => p.PictureId);
	    }

    }
}