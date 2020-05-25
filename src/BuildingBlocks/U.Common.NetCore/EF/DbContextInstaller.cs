using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.NetCore.EF;
using U.Common.NetCore.Mvc;

// ReSharper disable RedundantCaseLabel

namespace U.Common.NetCore.Database
{
    public static class DbContextInstaller
    {
        public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext
        {
            var dbOptions = configuration.GetOptions<DbOptions>("dbOptions");

            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            }
            services.AddSingleton(dbOptions);
            services.SelectContextProvider<TContext>(dbOptions);
            return services;
        }

        private static IServiceCollection SelectContextProvider<TContext>(this IServiceCollection services, DbOptions dbOptions)
            where TContext : DbContext
        {
            switch (dbOptions.Type)
            {
                case DbType.Npgsql:
                    services.AddDbContextPool<TContext>((serviceProvider, options) =>
                    {
                        options.UseNpgsql(dbOptions.Connection,
                            postgresOptions =>
                            {
                                postgresOptions.MigrationsAssembly(typeof(TContext).GetTypeInfo().Assembly.GetName()
                                    .Name);
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                                postgresOptions.EnableRetryOnFailure(3,
                                    TimeSpan.FromSeconds(5), new List<string>());
                            });
                    });
                    break;
                case DbType.Mssql:
                    services.AddDbContextPool<TContext>(options => { options.UseSqlServer(dbOptions.Connection); });
                    break;
                case DbType.Unknown:
                case DbType.InMemory:
                    services.AddDbContextPool<TContext>(options => { options.UseInMemoryDatabase("inMemory"); });
                    break;
                default:
                    throw new UnsupportedDatabaseException("Unsupported database type selected.");
            }

            return services;
        }
    }
}