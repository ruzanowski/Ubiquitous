using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.FetchService.Persistance.Configuration;
using U.FetchService.Persistance.Context;

// ReSharper disable RedundantCaseLabel

namespace U.FetchService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUmContext(this IServiceCollection services, IConfiguration configuration)
        {
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            configuration.GetSection(dbOptionsSection).Bind(dbOptions);

            if (dbOptions?.Connection is null)
            {
                throw new Exception("Database options are missing.");
            }
	        
            switch (dbOptions.Type)
            {
                case DbType.Npgsql:
                    services.AddDbContext<UmContext>(options =>
                    {
                        options.UseNpgsql(dbOptions.Connection);
                    });
                    break;
                case DbType.Mssql:
                    services.AddDbContext<UmContext>(options =>
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