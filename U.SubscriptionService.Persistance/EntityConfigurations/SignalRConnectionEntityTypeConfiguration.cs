using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using U.SubscriptionService.Domain;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Persistance.EntityConfigurations
{
    class SignalRConnectionEntityTypeConfiguration : IEntityTypeConfiguration<SignalRConnection>
    {
        public void Configure(EntityTypeBuilder<SignalRConnection> builder)
        {
            builder.ToTable("SignalRConnections", SubscriptionContext.DEFAULT_SCHEMA);

            builder.HasKey(o => new
            {
                o.UserId,
                o.ConnectionId
            });
        }
    }
}
