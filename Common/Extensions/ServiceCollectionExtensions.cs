using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using U.Common.Database;
using U.FetchService.Infrastructure;

// ReSharper disable RedundantCaseLabel

namespace U.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseContext<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            configuration.GetSection(dbOptionsSection).Bind(dbOptions);

            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            }
	        
            switch (dbOptions.Type)
            {
                case DbType.Npgsql:
                    services.AddDbContext<T>(options =>
                    {
                        options.UseNpgsql(dbOptions.Connection);
                    });
                    break;
                case DbType.Mssql:
                    services.AddDbContext<T>(options =>
                    {
                        options.UseSqlServer(dbOptions.Connection);
                    });
                    break;
                case DbType.Unknown:
                default:
                    throw new UnsupportedDatabaseException("Unsupported database type selected.");
                
            }

            services.AddSingleton(dbOptions);
        }

        public static void AddRabbitMq(this IServiceCollection services, IConfigurationSection section)
        {
            // RabbitMQ Configuration
            var options = new RawRabbitConfiguration();
            section.Bind(options);
        
            var client = BusClientFactory.CreateDefault(options);
            services.AddSingleton<IBusClient>(_ => client);
        }
        
        public static void AddLoggingBehaviour(this IServiceCollection services)
            => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

    }
}