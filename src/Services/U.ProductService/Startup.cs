using System;
using System.Threading.Tasks;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U.Common.NetCore.Auth;
using U.Common.NetCore.Cache;
using U.Common.NetCore.Consul;
using U.Common.NetCore.EF;
using U.Common.NetCore.Jaeger;
using U.Common.NetCore.Mvc;
using U.Common.NetCore.Swagger;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.ProductService.Application.Common.Mappings;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Application.Events.IntegrationEvents.EventHandling;
using U.ProductService.Application.Infrastructure;
using U.ProductService.Application.Infrastructure.Behaviours;
using U.ProductService.BackgroundService;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Middleware;
using U.ProductService.Persistance.Contexts;
using U.ProductService.Persistance.Repositories.Category;
using U.ProductService.Persistance.Repositories.Manufacturer;
using U.ProductService.Persistance.Repositories.Picture;
using U.ProductService.Persistance.Repositories.Product;

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
                .AddCustomPipelineBehaviours()
                .AddCustomMapper()
                .AddCustomServices()
                .AddSwagger()
                .AddConsulServiceDiscovery()
                .AddRedis()
                .AddJwt()
                .AddJaeger()
                .AddProductBackgroundService(Configuration);

            RegisterEventsHandlers(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var pathBase = app.UsePathBase(Configuration, _logger).Item2;
            app
                .UseExceptionMiddleware()
                .UseSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders()
                .UseAuthentication()
                .UseJwtTokenValidator()
                .UseRouting()
                .UseCors("CorsPolicy")
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });

            RegisterConsul(app, applicationLifetime, client);
            RegisterEvents(app);
            SeedAsync(app).Wait();
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

        private void RegisterConsul(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }

        private async Task SeedAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            try
            {
                await new ProductContextSeeder()
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

    public static class CustomServiceRegistrations
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services = services
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<ICategoryRepository, CategoryRepository>()
                .AddTransient<IManufacturerRepository, ManufacturerRepository>()
                .AddTransient<IPictureRepository, PictureRepository>()
                .AddIntegrationEventLog()
                .AddTransient<IProductIntegrationEventService, ProductIntegrationEventService>()
                .AddSingleton<IDomainEventsService, DomainEventsService>();

            return services;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ManufacturerMappingProfile());
                mc.AddProfile(new PictureMappingProfile());
            }).CreateMapper();
            services.AddTransient(x => mapper);

            return services;
        }

        public static IServiceCollection AddCustomPipelineBehaviours(this IServiceCollection services)
        {
             services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EventPublishBehaviour<,>));

            return services;
        }
    }
}