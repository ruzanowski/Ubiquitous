using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using U.Common.Behaviour;
using U.Common.Consul;
using U.Common.Mvc;
using U.EventBus.Abstractions;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.Notification.SignalR.IntegrationEvents;
using U.Notification.SignalR.IntegrationEvents.ProductAdded;
using U.Notification.SignalR.IntegrationEvents.ProductPublished;

namespace U.Notification.SignalR
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
                .AddEventBusRabbitMq(Configuration)
                .AddMediatR(typeof(Startup).GetTypeInfo().Assembly)
                .AddCustomPipelineBehaviours()
                .AddLogging()
                .AddCustomSwagger()
                .AddCustomMapper()
                .AddCustomServices()                
                .AddCustomConsul();

            RegisterEventsHandlers(services);
        }

        public void Configure(IApplicationBuilder app,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            var pathBase = app.UseCustomPathBase(Configuration, _logger).Item2;
            app.UseDeveloperExceptionPage()
                .UseMvcWithDefaultRoute()
                .UseCors("CorsPolicy")
                .UseCustomSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders();

            RegisterConsul(app, applicationLifetime, client);
            RegisterEvents(app);
        }

        private void RegisterEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductAddedIntegrationEvent, ProductAddedIntegrationEventHandler>();
            eventBus.Subscribe<ProductPublishedIntegrationEvent, ProductPublishedIntegrationEventHandler>();
        }

        private void RegisterEventsHandlers(IServiceCollection services)
        {
            services.AddTransient<ProductAddedIntegrationEventHandler>();
            services.AddTransient<ProductPublishedIntegrationEventHandler>();
        }

        private void RegisterConsul(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
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
            return services;
        }
        
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product HTTP API",
                    Version = "v1",
                    Description = "The Product Service HTTP API"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, string pathBase)
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json",
                        "ProductService V1");
                });

            return app;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
            {
            }).CreateMapper());

            return services;
        }
        
        public static IServiceCollection AddCustomPipelineBehaviours(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
             return services;
        }
    }
}