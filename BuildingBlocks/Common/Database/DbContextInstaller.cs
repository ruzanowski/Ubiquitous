using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Mvc;

// ReSharper disable RedundantCaseLabel

namespace U.Common.Database
{
    public static class DbContextInstaller
    {
        public static IServiceCollection AddDatabaseOptionsAsSingleton(this IServiceCollection services,
            IConfiguration configuration)
        {
            var dbOptions = configuration.GetOptions<DbOptions>("DbOptions");

            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            } 
            services.AddSingleton(dbOptions);
            return services;
        }
        
        public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            var dbOptions = services.BuildServiceProvider().GetService<DbOptions>();
            
            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            }

            switch (dbOptions.Type)
            {
                case DbType.Npgsql:
                    services.AddDbContext<TContext>(options =>
                    {
                        options.UseNpgsql(dbOptions.Connection,
                            postgresOptions =>
                            {
                                postgresOptions.MigrationsAssembly(typeof(TContext).GetTypeInfo().Assembly.GetName().Name);
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                postgresOptions.EnableRetryOnFailure(10,
                                    TimeSpan.FromSeconds(30), new List<string>());
                            });
                    });
                    break;
                case DbType.Mssql:
                    services.AddDbContext<TContext>(options => { options.UseSqlServer(dbOptions.Connection); });
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