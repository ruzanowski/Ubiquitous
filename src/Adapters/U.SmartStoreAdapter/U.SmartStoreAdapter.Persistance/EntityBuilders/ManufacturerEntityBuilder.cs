using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace SmartStore.Persistance.EntityBuilders
{

    public class ManufacturerEntityBuilder : IEntityTypeConfiguration<Manufacturer>
    {
	    public void Configure(EntityTypeBuilder<Manufacturer> builder)
	    {
		    builder.ToTable("Manufacturer").HasKey(a => a.Id);
		    builder.Property(x => x.Id).IsRequired();
		    builder.Property(m => m.Name).IsRequired().HasMaxLength(400);
		    builder.Property(m => m.Description);
	    }

    }
}