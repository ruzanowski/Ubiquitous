using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using U.Common.Mvc;
using U.NotificationService.Application.Builders.PendingNotifications;
using U.NotificationService.Application.SignalR;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.PeriodicSender
{
    public static class PeriodSenderExtensions
    {
        public static IServiceCollection AddBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            var backgroundServiceOptions = configuration.GetOptions<BackgroundServiceOptions>("backgroundService");

            var pendingEventService = new PendingEventsService();
            services.AddSingleton(backgroundServiceOptions);
            services.AddSingleton<IPendingEventsService>(pendingEventService);
            services.AddSingleton<IHostedService, NotificationSenderHostedService>(p =>
                new NotificationSenderHostedService(pendingEventService, backgroundServiceOptions,
                    p.GetRequiredService<PersistentHub>()));

            return services;
        }
    }
}