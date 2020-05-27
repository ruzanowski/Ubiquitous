using System;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using U.Common.NetCore.EF;
using U.Common.NetCore.Mvc;
using U.Common.NetCore.WebHost;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService
{
    public class Program
    {
        private static readonly string Namespace = typeof(Program).Namespace;

        private static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = SharedWebHost.GetConfiguration();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Log.Logger = SharedWebHost.CreateSerilog(configuration, env, AppName);
            try
            {
                Log.Information($"Configuring web host ({AppName})...");
                var host = SharedWebHost.BuildWebHost<Startup>(configuration, args, AppName);
                var dbOptions = configuration.GetOptions<DbOptions>("dbOptions");

                Log.Information($"Application started in mode: '{env?.ToLower()}'");


                if (dbOptions?.AutoMigration != null && dbOptions.AutoMigration)
                {
                    Log.Information($"Applying migrations ({AppName})...");

                    host.MigrateDbContext<NotificationContext>((_, __) => { });
                }


                Log.Information($"Starting web host ({AppName})...");
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Program terminated unexpectedly ({AppName})!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}