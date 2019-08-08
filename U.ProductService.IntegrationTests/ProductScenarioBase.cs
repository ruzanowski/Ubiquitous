using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using U.Common.WebHost;
using U.IntegrationEventLog;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.IntegrationTests
{
    public class ProductScenarioBase
    {
        protected static TestServer CreateServer()
        {
            TestServer testServer = null;
            try
            {
                var path = Assembly.GetAssembly(typeof(ProductScenarioBase))
                    .Location;
                var hostBuilder = new WebHostBuilder()
                    .UseContentRoot(Path.GetDirectoryName(path))
                    .ConfigureAppConfiguration(cb =>
                    {
                        cb.AddJsonFile("appsettings.json", optional: false)
                            .AddEnvironmentVariables();
                    }).UseStartup<Startup>();
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

        protected static class ProductService
        {
            private const string EndpointPrefix = "api/product-service";
            private const string ProductPrefix = "products";
            private static readonly string BaseUrl = $"/{EndpointPrefix}/{ProductPrefix}";
        
            public static readonly string QueryProducts = $"{BaseUrl}/query";
            public static readonly string CreateProduct = $"{BaseUrl}/create";
            public static readonly string QueryProduct = $"{BaseUrl}/query";
            public static readonly string UpdateProduct = $"{BaseUrl}/update";
        }
    }
}