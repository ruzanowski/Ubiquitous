using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.FetchService.Persistance.Configuration;
using U.SmartStoreAdapter.Application.Models.Exceptions;

// ReSharper disable RedundantCaseLabel

namespace U.FetchService.Extensions
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


    }
}