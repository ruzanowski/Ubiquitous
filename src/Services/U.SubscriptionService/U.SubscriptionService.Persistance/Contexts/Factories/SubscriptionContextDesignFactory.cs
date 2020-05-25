using Microsoft.EntityFrameworkCore.Design;
using U.Common.NetCore.Database;

namespace U.SubscriptionService.Persistance.Contexts.Factories
{
    public class SubscriptionContextDesignFactory : IDesignTimeDbContextFactory<SubscriptionContext>
    {
        public SubscriptionContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<SubscriptionContext>("../../../../");

            return new SubscriptionContext(optionsBuilder.Options);
        }
    }
}