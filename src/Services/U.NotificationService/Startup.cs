using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U.Common.Miscellaneous;
using U.Common.NetCore.Auth;
using U.Common.NetCore.Cache;
using U.EventBus.Abstractions;
using U.IntegrationEventLog;
using U.Common.NetCore.Consul;
using U.Common.NetCore.EF;
using U.Common.NetCore.Fabio;
using U.Common.NetCore.Jaeger;
using U.Common.NetCore.Mvc;
using U.Common.NetCore.Swagger;
using U.EventBus.Events.Product;
using U.EventBus.RabbitMQ;
using U.NotificationService.Application.Common.Builders.Query;
using U.NotificationService.Application.Common.Clients;
using U.NotificationService.Application.Common.Models;
using U.NotificationService.Application.EventHandlers;
using U.NotificationService.Application.Queries.GetCount;
using U.NotificationService.Application.Services.Operations;
using U.NotificationService.Application.SignalR;
using U.NotificationService.Application.SignalR.WelcomeNotifications;
using U.NotificationService.Infrastructure.Contexts;
using U.NotificationService.PeriodicSender;

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
                .AddConsulServiceDiscovery()
                .AddTypedHttpClient<ISubscriptionService>(GlobalConstants.SubscriptionServiceConsulRegisteredName)
                .AddCustomRedisAndSignalR()
                .AddCustomServices()
                .AddJwt()
                .AddRedis()
                .AddJaeger()
                .AddBackgroundService(Configuration);


            RegisterEventsHandlers(services);
        }

        public void Configure(IApplicationBuilder app,
            IHostApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseCors("CorsPolicy");

            var pathBase = app.UsePathBase(Configuration, _logger).Item2;
            app.UseHttpsRedirection()
                .UseSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders()
                .UseCookiePolicy();
            app.UseStaticFiles();

            app.UseJwtTokenValidator();

            app.UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapHub<BaseHub>("/signalr");
                });

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

        private void RegisterConsul(IApplicationBuilder app,
            IHostApplicationLifetime applicationLifetime,
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
            services.AddScoped<INotificationsService, NotificationOperations>();

            return services;
        }

        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc => { }).CreateMapper());

            return services;
        }

        private static readonly string SignalRSectionName = "signalr";

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