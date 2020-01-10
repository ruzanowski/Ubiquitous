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
using U.Common.Consul;
using U.Common.Mvc;
using U.EventBus.Abstractions;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.Common.Database;
using U.Common.Fabio;
using U.Common.Jaeger;
using U.Common.Jwt;
using U.Common.Redis;
using U.Common.Swagger;
using U.EventBus.Events.Product;
using U.NotificationService.Application.EventHandlers;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.Queries.GetCount;
using U.NotificationService.Application.SignalR;
using U.NotificationService.Application.SignalR.Services.Notifications;
using U.NotificationService.Application.SignalR.Services.QueryBuilder;
using U.NotificationService.Application.SignalR.Services.Service;
using U.NotificationService.Application.SignalR.Services.Subscription;
using U.NotificationService.Application.SignalR.Services.WelcomeNotifications;
using U.NotificationService.Infrastructure.Contexts;

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
                .AddMediatR(typeof(GetNotificationCount).GetTypeInfo().Assembly)
                .AddSwagger()
                .AddCustomMapper()
                .AddCustomServices()
                .AddConsulServiceDiscovery()
                .AddTypedHttpClient<ISubscriptionService>("u.subscription-service")
                .AddCustomRedisAndSignalR()
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
            app.UseStaticFiles();

            app.UseJwtTokenValidator();

            app.UseSignalR(routes => routes.MapHub<BaseHub>("/signalr"));

            app.UseMvcWithDefaultRoute();

            RegisterConsul(app, applicationLifetime, client);
            RegisterEvents(app);
        }

        private void RegisterEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductAddedSignalRIntegrationEvent, ProductAddedSignalRIntegrationEventHandler>();
            eventBus
                .Subscribe<ProductPublishedSignalRIntegrationEvent, ProductPublishedSignalRIntegrationEventHandler>();
            eventBus
                .Subscribe<ProductPropertiesChangedSignalRIntegrationEvent,
                    ProductPropertiesChangedSignalRIntegrationEventHandler>();
        }

        private void RegisterEventsHandlers(IServiceCollection services)
        {
            services.AddTransient<ProductAddedSignalRIntegrationEventHandler>();
            services.AddTransient<ProductPublishedSignalRIntegrationEventHandler>();
            services.AddTransient<ProductPropertiesChangedSignalRIntegrationEventHandler>();
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
            services.AddSingleton<PersistentHub>();
            services.AddScoped<IWelcomeNotificationsService, WelcomeNotificationsService>();
            services.AddScoped<INotificationQueryBuilder, NotificationQueryBuilder>();
            services.AddScoped<INotificationsService, Application.SignalR.Services.Notifications.NotificationsService>();

            return services;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc => { }).CreateMapper());

            return services;
        }

        private static string SignalRSectionName = "signalr";

        public static IServiceCollection AddCustomRedisAndSignalR(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var signalROptions = configuration.GetOptions<SignalROptions>(SignalRSectionName);

            services.TryAddSingleton(signalROptions);

            services
                .AddSignalR(options => { options.EnableDetailedErrors = true; })
                .AddJsonProtocol()
                .AddMessagePackProtocol()
                .AddStackExchangeRedis(signalROptions.RedisConnectionString);

            return services;
        }
    }
}