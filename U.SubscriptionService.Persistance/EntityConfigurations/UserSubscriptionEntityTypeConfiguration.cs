using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.SubscriptionService.Domain;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Persistance.EntityConfigurations
{
    class UserSubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.ToTable("UserSubscription", SubscriptionContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x=>x.Connections)
                .WithOne()
                .IsRequired(false);

            builder.OwnsOne(o => o.Preferences);

        }
    }
}
