using Microsoft.Extensions.DependencyInjection;
using U.IntegrationEventLog.Services;

namespace U.IntegrationEventLog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEventLog(this IServiceCollection services)
        {
            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();

            return services;
        }
    }
}