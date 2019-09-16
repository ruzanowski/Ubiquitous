using System;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using U.Common.Database;
using U.Common.Mvc;
using U.Common.WebHost;
using U.IntegrationEventLog;
using U.ProductService.Persistance;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService
{
    public class Program
    {
        private static readonly string Namespace = typeof(Program).Namespace;

        private static readonly string AppName =
            Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = SharedWebHost.GetConfiguration();

            Log.Logger = SharedWebHost.CreateSerilogLogger(configuration, AppName);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = SharedWebHost.BuildWebHost<Startup>(configuration, args);
                var dbOptions = configuration.GetOptions<DbOptions>("DbOptions");

                Log.Information(
                    $"Application started in mode: '{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower()}'");

                if (dbOptions?.AutoMigration != null && dbOptions.AutoMigration)
                {
                    Log.Information("Applying migrations ({ApplicationContext})...", AppName);

                    host.MigrateDbContext<ProductContext>((_, __) => { })
                        .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });
                }

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}