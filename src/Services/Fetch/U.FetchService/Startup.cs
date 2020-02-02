using System.Reflection;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Consul;
using U.Common.Fabio;
using U.Common.Jaeger;
using U.Common.Mvc;
using U.EventBus.RabbitMQ;
using U.FetchService.BackgroundServices;
using U.FetchService.Commands.FetchProducts;
using U.FetchService.Services;

namespace U.FetchService
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc()
                .AddCustomMediatR()
                .AddEventBusRabbitMq(Configuration)
                .AddConsulServiceDiscovery()
                .AddTypedHttpClient<ISmartStoreAdapter>("u.smartstore-adapter")
                .AddBackgroundService(Configuration)
                .AddJaeger();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UsePathBase(Configuration, _logger).Item1
                .UseMvcWithDefaultRoute()
                .UseServiceId()
                .UseForwardedHeaders();

            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(FetchProductsCommand).GetTypeInfo().Assembly);
            return services;
        }

        public static IServiceCollection AddBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            var backgroundService = configuration.GetOptions<BackgroundServiceOptions>("backgroundService");
            services.AddSingleton(backgroundService);
            services.AddHostedService<ProductsUpdateWorkerHostedService>();
            return services;
        }
    }
}