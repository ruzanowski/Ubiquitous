using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Database;

// ReSharper disable RedundantCaseLabel

namespace U.Common.Installers
{
    public static class DbContextInstaller
    {
        public static IServiceCollection AddDatabaseOptionsAsSingleton(this IServiceCollection services,
            IConfiguration configuration)
        {
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            configuration.GetSection(dbOptionsSection).Bind(dbOptions);

            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            } 
            services.AddSingleton(dbOptions);
            return services;
        }
        
        public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services)
            where T : DbContext
        {
            var dbOptions = services.BuildServiceProvider().GetService<DbOptions>();
            
            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            }

            switch (dbOptions.Type)
            {
                case DbType.Npgsql:
                    services.AddDbContext<T>(options => { options.UseNpgsql(dbOptions.Connection); });
                    break;
                case DbType.Mssql:
                    services.AddDbContext<T>(options => { options.UseSqlServer(dbOptions.Connection); });
                    break;
                case DbType.Unknown:
                default:
                    throw new UnsupportedDatabaseException("Unsupported database type selected.");
            }

            services.AddSingleton(dbOptions);
            return services;
        }
    }
}