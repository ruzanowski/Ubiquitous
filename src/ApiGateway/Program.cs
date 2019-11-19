using Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Administration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using U.Common.Consul;
using U.Common.Jwt;
using U.Common.Mvc;

namespace U.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:4500")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true,
                            true)
                        .AddJsonFile("ocelot.json", false, false)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    services.AddConsulServiceDiscovery()
                        .AddJwt();

                    services.AddOcelot()
                        .AddConsul()
                        .AddAdministration("/administration", "secret");

                    services.AddCustomMvc();
                })
                .Configure(app =>
                {
                    app.UseCors
                    (b => b
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                    );
                    app.UseServiceId();
                    app.UseOcelot().Wait();

                    var consulServiceId = app.UseConsulServiceDiscovery();

                    var applicationLifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
                    var client = app.ApplicationServices.GetService<IConsulClient>();
                    applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });

                })
                .Build();
        }
    }
}
