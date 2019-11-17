﻿using System.Reflection;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc()
                .AddCustomMediatR()
                .AddEventBusRabbitMq(Configuration)
                .AddConsul()
                .RegisterServiceForwarder<ISmartStoreAdapter>("u.smartstore-adapter")
                .AddUpdateWorkerHostedService(Configuration);
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UsePathBase(Configuration, _logger).Item1
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

        public static IServiceCollection AddUpdateWorkerHostedService(this IServiceCollection services, IConfiguration configuration)
        {
            var backgroundService = configuration.GetOptions<BackgroundServiceOptions>("backgroundService");
            services.AddSingleton(backgroundService);
            services.AddHostedService<ProductsUpdateWorkerHostedService>();
            return services;
        }
    }
}