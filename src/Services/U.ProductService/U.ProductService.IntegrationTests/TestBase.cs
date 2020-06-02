using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.NetCore.WebHost;
using U.IntegrationEventLog;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.IntegrationTests
{
    public partial class TestBase
    {
        protected static TestServer CreateServer()
        {
            TestServer testServer = null;
            try
            {
                var path = Assembly.GetAssembly(typeof(Startup))?.Location;
                var hostBuilder = new WebHostBuilder()
                    .UseContentRoot(Path.GetDirectoryName(path))
                    .ConfigureAppConfiguration(cb =>
                    {
                        cb.AddJsonFile("appsettings.IT.json", optional: false)
                            .AddEnvironmentVariables();

                    })
                    .UseStartup<Startup>();

                testServer = new TestServer(hostBuilder);

                testServer.Host
                .MigrateDbContext<ProductContext>((_, __) => { })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

                return testServer;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return testServer;
        }
    }
}