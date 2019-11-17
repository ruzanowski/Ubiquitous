﻿using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Consul;
using U.Common.Mvc;
using U.EventBus.Abstractions;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.Common.Database;
using U.Common.Redis;
using U.Common.Swagger;
using U.NotificationService.Application.Hub;
using U.NotificationService.Application.IntegrationEvents.ProductAdded;
using U.NotificationService.Application.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.Application.IntegrationEvents.ProductPublished;
using U.NotificationService.Infrastracture.Contexts;

namespace U.NotificationService
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddDatabaseContext<NotificationContext>(Configuration)
                .AddEventBusRabbitMq(Configuration)
                .AddMediatR(typeof(Startup).GetTypeInfo().Assembly)
                .AddLogging()
                .AddSwagger()
                .AddCustomMapper()
                .AddCustomServices()
                .AddConsul()
                .AddCustomRedisAndSignalR();

            RegisterEventsHandlers(services);
        }

        public void Configure(IApplicationBuilder app,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            var pathBase = app.UsePathBase(Configuration, _logger).Item2;
            app.UseDeveloperExceptionPage()
                .UseMvcWithDefaultRoute()
                .UseCors("CorsPolicy")
                .UseSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders();

            app.UseSignalR(routes => routes.MapHub<BaseHub>("/signalr"));

            RegisterConsul(app, applicationLifetime, client);
            RegisterEvents(app);
        }

        private void RegisterEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductAddedIntegrationEvent, ProductAddedIntegrationEventHandler>();
            eventBus.Subscribe<ProductPublishedIntegrationEvent, ProductPublishedIntegrationEventHandler>();
            eventBus.Subscribe<ProductPropertiesChangedIntegrationEvent, ProductPropertiesChangedIntegrationEventHandler>();
        }

        private void RegisterEventsHandlers(IServiceCollection services)
        {
            services.AddTransient<ProductAddedIntegrationEventHandler>();
            services.AddTransient<ProductPublishedIntegrationEventHandler>();
            services.AddTransient<ProductPropertiesChangedIntegrationEventHandler>();
        }

        private void RegisterConsul(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var consulServiceId = app.UseCustomConsul();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomServiceRegistrations
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddIntegrationEventLog();
            services.AddSingleton<PersistentHub>();
            return services;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc => { }).CreateMapper());

            return services;
        }
    }
}