using U.SmartStoreAdapter.Domain.Entities.Seo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartStore.Persistance.EntityBuilders.Seo
{
    public class 
        UrlRecordMap : IEntityTypeConfiguration<UrlRecord>
    {
       
        public void Configure(EntityTypeBuilder<UrlRecord> builder)
        {
            builder.ToTable("UrlRecord").HasKey(x=>x.Id);
            builder.Property(x => x.EntityName).IsRequired().HasMaxLength(400);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(400);
        }
    }
}