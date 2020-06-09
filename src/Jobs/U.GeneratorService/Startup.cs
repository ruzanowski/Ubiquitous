using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U.Common.Miscellaneous;
using U.Common.NetCore.Consul;
using U.Common.NetCore.Fabio;
using U.Common.NetCore.Jaeger;
using U.Common.NetCore.Mvc;
using U.GeneratorService.BackgroundServices;
using U.GeneratorService.Services;

namespace U.GeneratorService
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
                .AddConsulServiceDiscovery()
                .AddTypedHttpClient<ISmartStoreAdapter>(GlobalConstants.SmartStoreConsulRegisteredName)
                .AddUpdateWorkerHostedService(Configuration)
                .AddCustomServices()
                .AddJaeger();

        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime, IConsulClient client)
        {

            app
                .UsePathBase(Configuration, _logger).Item1
                .UseCors("CorsPolicy")
                .UseServiceId()
                .UseForwardedHeaders()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });

            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomExtensions
    {
        public static IServiceCollection AddUpdateWorkerHostedService(this IServiceCollection services, IConfiguration configuration)
        {
            var backgroundService = configuration.GetOptions<BackgroundServiceOptions>("backgroundService");
            services.AddSingleton(backgroundService);
            services.AddHostedService<FakeProductsGeneratorWorkerHostedService>();
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IProductGenerator, FakeProductGenerator>();
            return services;
        }
    }
}