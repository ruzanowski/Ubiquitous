using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Infrastructure.EntityConfigurations
{
    class ConfirmationEntityTypeConfiguration : IEntityTypeConfiguration<Confirmation>
    {
        public void Configure(EntityTypeBuilder<Confirmation> builder)
        {
            builder.ToTable("Notification_confirmation", NotificationContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.User).IsRequired();
            builder.Property(x => x.ConfirmationDate).IsRequired();
            builder.Property(x => x.NotificationId).IsRequired();
            builder.HasIndex(x => x.User);
        }
    }
}
