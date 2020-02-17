using System;
using U.SmartStoreAdapter.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Media
{
	public class MediaStorageMap : IEntityTypeConfiguration<MediaStorage>
    {
	    public void Configure(EntityTypeBuilder<MediaStorage> builder)
	    {
		    builder.ToTable("MediaStorage").HasKey(a => a.Id);
		    builder.Property(x => x.Id).ValueGeneratedOnAdd();
		    builder.Property(x => x.Data).IsRequired().HasMaxLength(Int32.MaxValue);
	    }
    }

	
	
}
