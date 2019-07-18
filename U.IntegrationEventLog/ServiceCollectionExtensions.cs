using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Database;
using U.IntegrationEventLog.Services;

namespace U.IntegrationEventLog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEventLog(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = services.BuildServiceProvider().GetService<DbOptions>();
            
            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            }
            
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseNpgsql(configuration["IntegrationEventLogConnection"],
                     npsqlOptions =>
                    {
                        npsqlOptions.MigrationsAssembly(typeof(IServiceCollection).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        npsqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30),null);
                    });
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });

            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();

            return services;
        }
    }
}