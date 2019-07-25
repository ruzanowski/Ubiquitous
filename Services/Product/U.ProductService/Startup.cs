using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using U.Common.Behaviours;
using U.Common.Installers;
using U.EventBus.Abstractions;
using U.EventBus.RabbitMQ;
using U.FetchService.Api.IntegrationEvents;
using U.IntegrationEventLog;
using U.ProductService.Application.Behaviours;
using U.ProductService.Application.Commands;
using U.ProductService.Application.IntegrationEvents;
using U.ProductService.Application.IntegrationEvents.EventHandling;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Middleware;
using U.ProductService.Persistance.Contexts;
using U.ProductService.Persistance.Repositories;

namespace U.ProductService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddDatabaseOptionsAsSingleton(Configuration)
                .AddDatabaseContext<ProductContext>()
                .AddEventBusRabbitMq(Configuration)
                .AddIntegrationEventLog(Configuration)
                .AddMediatR(typeof(CreateProductCommand).GetTypeInfo().Assembly)
                .AddCustomPipelineBehaviours()
                .AddLogging()
                .AddCustomSwagger()
                .AddCustomMapper()
                .AddCustomServices();

            RegisterEventsHandlers(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UseDeveloperExceptionPage()
                .UseMvcWithDefaultRoute()
                .UseCors("CorsPolicy")
                .AddExceptionMiddleware()
                .UseCustomSwagger(pathBase);

            RegisterEvents(app);
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
    }

    public static class CustomServiceRegistrations
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient<IProductIntegrationEventService, ProductIntegrationEventService>();

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
                //maps
            }).CreateMapper());

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // AddAsync framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices(); //Injecting Controllers themselves thru DI
            //For further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;
        }
        
        public static IServiceCollection AddCustomPipelineBehaviours(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
             return services;
        }
    }
}