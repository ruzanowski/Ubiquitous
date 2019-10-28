using Microsoft.EntityFrameworkCore.Design;
using U.Common.Database;

namespace U.NotificationService.Infrastracture.Contexts.Factories
{
    public class NotificationContextDesignFactory : IDesignTimeDbContextFactory<NotificationContext>
    {
        public NotificationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<NotificationContext>("../../../../U.NotificationService");
            
            return new NotificationContext(optionsBuilder.Options);
        }
    }
}