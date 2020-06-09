using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Serilog;
using U.Common.NetCore.Elasticsearch;

namespace U.Common.NetCore.WebHost
{
    public static class SharedWebHost
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost,
            Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = webHost.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}",
                    typeof(TContext).Name);

                var retry = Policy
                    .Handle<SqlException>()
                    .Or<NpgsqlException>()
                    .WaitAndRetry(new[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8)
                    });

                retry.Execute(() => InvokeSeeder(seeder, context, services));

                logger.LogInformation("Migrated database associated with context {DbContextName}",
                    typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An error occurred while migrating the database used on context {DbContextName}",
                    typeof(TContext).Name);
            }

            return webHost;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
            IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }

        public static IWebHost BuildWebHost<TStartup>(IConfiguration configuration, string[] args, string appName)
            where TStartup : class
            => Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .UseStartup<TStartup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();

        public static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower()}.json",
                    true, true)
                .AddEnvironmentVariables().Build();


        public static Serilog.ILogger CreateSerilog(IConfiguration configuration, string env, string appName)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .WriteToSinks(configuration)
                .CreateLogger();
        }

        private static LoggerConfiguration WriteToSinks(this LoggerConfiguration loggerConfiguration,
            IConfiguration configuration) =>
            loggerConfiguration
                .WriteTo.Console()
                .ElasticsearchSink(configuration);
    }
}