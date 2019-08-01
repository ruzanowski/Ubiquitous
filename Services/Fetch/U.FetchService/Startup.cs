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
using U.Common.Mvc;
using U.Common.RestEase;
using U.EventBus.RabbitMQ;
using U.FetchService.BackgroundServices;
using U.FetchService.Commands.UpdateProducts;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc()
                .AddCustomMediatR()
                .AddLoggingBehaviour()
                .AddEventBusRabbitMq(Configuration)
                .AddCustomConsul()
                .AddCustomFabio()
                .RegisterServiceForwarder<ISmartStoreAdapter>("u.smartstore-adapter")
                .AddHostedService<ProductsUpdateWorkerHostedService>();
        }     

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(UpdateProductsCommand).GetTypeInfo().Assembly);
            return services;
        }
    }
}