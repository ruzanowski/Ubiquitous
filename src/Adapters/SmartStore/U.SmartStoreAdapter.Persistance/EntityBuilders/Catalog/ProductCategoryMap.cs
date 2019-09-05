//using System.Data.Entity.ModelConfiguration;
//using U.SmartStoreAdapter.Domain.Entities.Catalog;
//

using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
{
	public class ProductCategoryMap : IEntityTypeConfiguration<ProductCategory>
    {
	    public void Configure(EntityTypeBuilder<ProductCategory> builder)
	    {
		    
		    builder.ToTable("Product_Category_Mapping").HasKey(a => a.Id);
		    builder.Property(x => x.Id).ValueGeneratedOnAdd();
		    
		    builder.HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId);

		    builder.HasOne(pc => pc.Product)
			    .WithMany(p => p.ProductCategories)
			    .HasForeignKey(pc => pc.ProductId);

	    }
    }
 



}