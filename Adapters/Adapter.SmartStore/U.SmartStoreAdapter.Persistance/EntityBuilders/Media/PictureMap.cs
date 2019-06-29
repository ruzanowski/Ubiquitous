using U.SmartStoreAdapter.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Media
{
	public class PictureMap : IEntityTypeConfiguration<Picture>
    {
	    public void Configure(EntityTypeBuilder<Picture> builder)
	    {
		    builder.ToTable("Picture").HasKey(a => a.Id);
		    builder.Property(x => x.MimeType).IsRequired().HasMaxLength(40);
		    builder.Ignore(x => x.MediaStorage);
	    }
    }
}