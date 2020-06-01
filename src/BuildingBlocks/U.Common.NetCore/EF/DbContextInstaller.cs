using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.NetCore.Mvc;

// ReSharper disable RedundantCaseLabel

namespace U.Common.NetCore.EF
{
    public static class DbContextInstaller
    {
        public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext
        {
            ServiceIdentity serviceIdentity;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                serviceIdentity = serviceProvider.GetService<ServiceIdentity>();
            }

            var dbOptions = configuration.GetOptions<DbOptions>("dbOptions");

            if (dbOptions.InTests)
            {
                dbOptions.Connection = Environment.GetEnvironmentVariable($"{serviceIdentity.Name}_test_connection");
            }

            if (dbOptions is null)
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
                                postgresOptions.MigrationsAssembly(
                                    typeof(TContext).GetTypeInfo().Assembly.GetName()
                                        .Name);
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