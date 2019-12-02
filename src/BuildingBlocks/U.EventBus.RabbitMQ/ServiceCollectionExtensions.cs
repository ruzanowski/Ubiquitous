using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using U.Common.Mvc;
using U.EventBus.Abstractions;

namespace U.EventBus.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        private const string RabbitSectionName = "rabbit";
        public static IServiceCollection AddEventBusRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbit = configuration.GetOptions<RabbitOptions>(RabbitSectionName);

            if(!rabbit.Enabled)
                return services;

            services.AddSingleton(rabbit);

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(rabbit.EventBusRetryCount))
                {
                    retryCount = int.Parse(rabbit.EventBusRetryCount);
                }

                return new EventBusRabbitMQ(rabbitMqPersistentConnection, logger, services.BuildServiceProvider(),
                    eventBusSubcriptionsManager, rabbit.SubscriptionClientName, retryCount);
            });

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory
                {
                    HostName = rabbit.EventBusConnection,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(rabbit.EventBusUserName))
                {
                    factory.UserName = rabbit.EventBusUserName;
                }

                if (!string.IsNullOrEmpty(rabbit.EventBusPassword))
                {
                    factory.Password = rabbit.EventBusPassword;
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(rabbit.EventBusRetryCount))
                {
                    retryCount = int.Parse(rabbit.EventBusRetryCount);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}