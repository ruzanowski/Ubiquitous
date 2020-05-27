using Microsoft.EntityFrameworkCore.Design;
using U.Common.NetCore.EF;

namespace U.NotificationService.Infrastructure.Contexts.Factories
{
    public class NotificationContextDesignFactory : IDesignTimeDbContextFactory<NotificationContext>
    {
        public NotificationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<NotificationContext>("../../../../");

            return new NotificationContext(optionsBuilder.Options);
        }
    }
}