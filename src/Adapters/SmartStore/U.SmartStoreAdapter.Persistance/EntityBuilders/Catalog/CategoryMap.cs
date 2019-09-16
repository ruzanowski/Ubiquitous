using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
{
	public class CategoryMap : IEntityTypeConfiguration<Category>
    {
	    public void Configure(EntityTypeBuilder<Category> builder)
	    {
		    builder.ToTable("Category").HasKey(a => a.Id);
		    builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
		    builder.Property(x => x.Name).HasMaxLength(400);
		    builder.Property(x => x.FullName).HasMaxLength(400);
		    builder.Property(x => x.BottomDescription).HasMaxLength(400);
		    builder.Property(x => x.Description);
		    builder.Property(x => x.MetaKeywords).HasMaxLength(400);
		    builder.Property(x => x.MetaTitle).HasMaxLength(400);
		    builder.Property(x => x.PageSizeOptions).HasMaxLength(200);
		    builder.Property(x => x.Alias).HasMaxLength(100);
		    builder.HasOne(x => x.Picture).WithMany().HasForeignKey(x => x.PictureId);
	    }
    }
    
    
    
}
