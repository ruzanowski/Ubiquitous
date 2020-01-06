using System;
using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OpenTracing;
using RawRabbit.Instantiation;
using StackExchange.Redis;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Jaeger;
using U.Common.Jwt;
using U.Common.Mvc;
using U.Common.Redis;
using U.Common.Swagger;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.EventBus.Events.Product;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.ProductService.Application.Common.Mapping;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Application.Events.IntegrationEvents.EventHandling;
using U.ProductService.Application.Infrastructure;
using U.ProductService.Application.Infrastructure.Behaviours;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Domain;
using U.ProductService.Middleware;
using U.ProductService.Persistance.Contexts;
using U.ProductService.Persistance.Repositories;

namespace U.ProductService
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
                .AddDatabaseContext<ProductContext>(Configuration)
                .AddDatabaseContext<IntegrationEventLogContext>(Configuration)
                .AddEventBusRabbitMq(Configuration)
                .AddMediatR(typeof(CreateProductCommand).GetTypeInfo().Assembly)
                .AddCustomPipelineBehaviours()
                .AddCustomMapper()
                .AddCustomServices()
                .AddLogging()
                .AddSwagger()
                .AddConsulServiceDiscovery()
                .AddRedis()
                .AddJwt()
                .AddJaeger();


            RegisterEventsHandlers(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var pathBase = app.UsePathBase(Configuration, _logger).Item2;
            app.UseDeveloperExceptionPage()
                .UseCors("CorsPolicy")
                .AddExceptionMiddleware()
                .UseSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders()
                .UseAuthentication()
                .UseJwtTokenValidator()
                .UseMvc();

            RegisterConsul(app, applicationLifetime, client);
            RegisterEvents(app);
            SeedAsync(app);
        }

        private void RegisterEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NewProductFetchedIntegrationEvent, NewProductFetchedIntegrationEventHandler>();
        }

        private void RegisterEventsHandlers(IServiceCollection services)
        {
            services.AddTransient<NewProductFetchedIntegrationEventHandler>();
        }

        private void RegisterConsul(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }

        private void SeedAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                try
                {
                    new ProductContextSeeder()
                        .SeedAsync(serviceScope.ServiceProvider.GetRequiredService<ProductContext>(),
                            serviceScope.ServiceProvider.GetRequiredService<DbOptions>(),
                            serviceScope.ServiceProvider.GetRequiredService<ILogger<ProductContextSeeder>>());
                }
                catch (Exception ex)
                {
                    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }

    public static class CustomServiceRegistrations
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services = services.AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<ICategoryRepository, CategoryRepository>()
                .AddTransient<IManufacturerRepository, ManufacturerRepository>()
                .AddIntegrationEventLog()
                .AddTransient<IProductIntegrationEventService, ProductIntegrationEventService>();

            return services;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ManufacturerMappingProfile());
            }).CreateMapper());

            return services;
        }

        public static IServiceCollection AddCustomPipelineBehaviours(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EventPublishBehaviour<,>));

            return services;
        }
    }
}