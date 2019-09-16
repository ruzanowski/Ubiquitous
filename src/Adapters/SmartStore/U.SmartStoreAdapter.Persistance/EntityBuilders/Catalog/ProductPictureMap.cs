using U.SmartStoreAdapter.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Catalog
{
	
       public class ProductPictureMap : IEntityTypeConfiguration<ProductPicture>
    {
	    public void Configure(EntityTypeBuilder<ProductPicture> builder)
	    {

		    builder.ToTable("Product_Picture_Mapping").HasKey(a => a.Id);
		    builder.Property(x => x.Id).ValueGeneratedOnAdd();

		    builder.HasOne(p => p.Picture) 
			    .WithMany(p => p.ProductPictures)
			    .HasForeignKey(x => x.PictureId)
			    .IsRequired();

		    builder.HasOne(p => p.Product)
			    .WithMany(p => p.ProductPictures)
			    .HasForeignKey(x => x.ProductId)
			    .IsRequired();

	    }
    }
}