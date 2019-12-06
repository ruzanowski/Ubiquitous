using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Infrastructure.EntityConfigurations
{
    class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification", NotificationContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.IntegrationEvent).IsRequired();

            builder.HasMany(x => x.Confirmations)
                .WithOne()
                .HasForeignKey(x => x.NotificationId)
                .IsRequired();
        }
    }
}
