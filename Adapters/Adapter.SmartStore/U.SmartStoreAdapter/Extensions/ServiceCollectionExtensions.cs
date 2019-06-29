using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartStore.Persistance.Context;
using U.FetchService.Persistance.Configuration;

// ReSharper disable RedundantCaseLabel

namespace U.SmartStoreAdapter.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="Exception"></exception>
        public static void AddSmartContext(this IServiceCollection services, IConfiguration configuration)
        {
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            configuration.GetSection(dbOptionsSection).Bind(dbOptions);

            if (dbOptions.Connection is null)
            {
                throw new Exception("Database options are missing.");
            }
	        
            switch (dbOptions.Type)
            {
                case DbType.Npgsql:
                    services.AddDbContext<SmartStoreContext>(options =>
                    {
                        options.UseNpgsql(dbOptions.Connection);
                    });
                    break;
                case DbType.Mssql:
                    services.AddDbContext<SmartStoreContext>(options =>
                    {
                        options.UseSqlServer(dbOptions.Connection);
                    });
                    break;
                case DbType.Unknown:
                default:
                    throw new Exception("Unsupported database type selected.");
                
            }
        }
    }
}