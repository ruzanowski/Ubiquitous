using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Fabio;
using U.Common.Jwt;
using U.Common.Monitoring.Jaeger;
using U.Common.Mvc;
using U.Common.Redis;
using U.Common.Swagger;
using U.EventBus.Abstractions;
using U.EventBus.Events.Product;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.SubscriptionService.Application.MappingProfiles;
using U.SubscriptionService.Application.Multiplexers;
using U.SubscriptionService.Application.Query;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService
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
                .AddDatabaseContext<SubscriptionContext>(Configuration)
                .AddEventBusRabbitMq(Configuration)
                .AddMediatR(typeof(MyPreferencesQuery).GetTypeInfo().Assembly)
                .AddSwagger()
                .AddCustomMapper()
                .AddCustomServices()
                .AddConsulServiceDiscovery()
                .AddJwt()
                .AddRedis()
                .AddJaeger();

            RegisterEventsHandlers(services);
        }

        public void Configure(IApplicationBuilder app,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseCors("CorsPolicy");

            var pathBase = app.UsePathBase(Configuration, _logger).Item2;
            app.UseDeveloperExceptionPage()
                .UseSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders()
                .UseCookiePolicy();


            app.UseJwtTokenValidator();

            app.UseMvcWithDefaultRoute();

            RegisterConsul(app, applicationLifetime, client);
            RegisterEvents(app);
        }

        private void RegisterEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductAddedIntegrationEvent, ProductAddedMultiplexingIntegrationEventHandler>();
            eventBus.Subscribe<ProductPublishedIntegrationEvent, ProductPublishedMultiplexingIntegrationEventHandler>();
            eventBus.Subscribe<ProductPropertiesChangedIntegrationEvent, ProductPropertiesChangedMultiplexingIntegrationEventHandler>();
        }

        private void RegisterEventsHandlers(IServiceCollection services)
        {
            services.AddTransient<ProductAddedMultiplexingIntegrationEventHandler>();
            services.AddTransient<ProductPublishedMultiplexingIntegrationEventHandler>();
            services.AddTransient<ProductPropertiesChangedMultiplexingIntegrationEventHandler>();
        }

        private void RegisterConsul(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomServiceRegistrations
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddIntegrationEventLog();

            return services;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new MultiplexMappingProfile())).CreateMapper());

            return services;
        }
    }
}