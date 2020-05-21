using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace SmartStore.Persistance.EntityBuilders
{
	public class CategoryEntityBuilder : IEntityTypeConfiguration<Category>
    {
	    public void Configure(EntityTypeBuilder<Category> builder)
	    {
		    builder.ToTable("Category").HasKey(a => a.Id);
		    builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
		    builder.Property(x => x.Name).HasMaxLength(400);
		    builder.Property(x => x.Description);
	    }
    }
}
