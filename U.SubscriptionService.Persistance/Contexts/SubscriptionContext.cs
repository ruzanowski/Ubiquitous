using Microsoft.EntityFrameworkCore;

namespace U.SubscriptionService.Persistance.Contexts
{
    public class SubscriptionContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "Subscriptions";


        public SubscriptionContext(DbContextOptions<SubscriptionContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
