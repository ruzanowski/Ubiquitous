using Microsoft.EntityFrameworkCore;
using U.Common.Subscription;
using U.SubscriptionService.Domain;
using U.SubscriptionService.Persistance.EntityConfigurations;

namespace U.SubscriptionService.Persistance.Contexts
{
    public class SubscriptionContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "Subscriptions";

        public DbSet<SignalRConnection> SignalRConnections { get; set; }
        public DbSet<UserSubscription> UsersSubscription { get; set; }

        public SubscriptionContext(DbContextOptions<SubscriptionContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SignalRConnectionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserSubscriptionEntityTypeConfiguration());
        }

    }
}
