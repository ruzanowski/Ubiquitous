using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using U.Common.EventPublisher;
using U.FetchService.Persistance.Messaging;

namespace U.FetchService.Extensions
{
    public static class RawRabbitInstaller
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfigurationSection section)
        {
            var options = new RawRabbitConfiguration();
            section.Bind(options);
        
            services.AddRawRabbit(new RawRabbitOptions
            {
                ClientConfiguration = options
            });
            services.AddSingleton<IEventPublisher, RabbitEventPublisher>();
        }
    }
    
    public static class RabbitListenersInstaller
    {
        public static void UseRabbitListeners(this IApplicationBuilder app, List<Type> eventTypes)
        {
            app.ApplicationServices.GetRequiredService<RabbitEventListener>().ListenTo(eventTypes);
        }
    }
}