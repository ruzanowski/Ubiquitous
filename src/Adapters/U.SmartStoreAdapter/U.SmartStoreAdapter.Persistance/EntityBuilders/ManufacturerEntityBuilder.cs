using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
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