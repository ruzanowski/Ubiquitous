using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using U.Common.Consul;
using U.Common.Mvc;
using U.Common.RestEase;
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
                .AddCustomConsul()
                .RegisterServiceForwarder<ISmartStoreAdapter>("u.smartstore-adapter")
                .AddUpdateWorkerHostedService(Configuration)
                .AddCustomServices();
        }     

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseCustomPathBase(Configuration, _logger).Item1
                .UseDeveloperExceptionPage()
                .UseMvcWithDefaultRoute()
                .UseServiceId()
                .UseForwardedHeaders();
            
            var consulServiceId = app.UseCustomConsul();
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