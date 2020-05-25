using System;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using U.Common.NetCore.WebHost;

namespace U.FetchService
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
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = SharedWebHost.BuildWebHost<Startup>(configuration, args, AppName);
                Log.Information(
                    $"Application started in mode: '{env?.ToLower()}'");
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