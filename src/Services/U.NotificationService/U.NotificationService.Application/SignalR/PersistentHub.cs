using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.EventBus.Events;
using U.NotificationService.Application.Common.Models;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

// ReSharper disable UnusedMember.Global

namespace U.NotificationService.Application.SignalR
{
    public class PersistentHub
    {
        private readonly IHubContext<BaseHub> _hubContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PersistentHub> _logger;

        private NotificationContext Context => _serviceProvider.CreateScope().ServiceProvider.GetService<NotificationContext>();

        public PersistentHub(IHubContext<BaseHub> hubContext,
            ILogger<PersistentHub> logger,
            IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task SaveManyAndSendToAllAsync(IList<IntegrationEvent> events)
        {
            IList<Notification> notifications = new List<Notification>();

            foreach (var eventWithName in events)
            {
                var notification = new Notification(eventWithName);
                notification.IncrementProcessedTimes();
                notifications.Add(notification);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Context.BulkInsert(notifications);

            foreach (var notification in notifications)
            {
                var notificationDto =
                    NotificationDto.Factory.GlobalVolatileNotification(notification);

                await _hubContext.Clients.All.SendAsync(notification.MethodTag, notificationDto);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            _logger.LogInformation($"Saved {notifications.Count} notifications in {elapsedMs} ms");
        }

        public async Task SaveAndSendToAllAsync<T>(string methodTag, T @event) where T : IntegrationEvent
        {
            var notification = new Notification(@event);
            notification.IncrementProcessedTimes();

            await Context.AddAsync(notification);
            await Context.SaveChangesAsync();

            var notificationDto = NotificationDto.Factory.GlobalVolatileNotification(notification);

            await _hubContext.Clients.All
                .SendAsync(methodTag, notificationDto);
        }

        public async Task SaveAndSendAsync<T>(string methodTag, string connectionId, Guid userId, T @event)
            where T : IntegrationEvent
        {
            var notification = new Notification(@event);
            notification.IncrementProcessedTimes();

            await Context.AddAsync(notification);
            await Context.SaveChangesAsync();

            var notificationDto =
                NotificationDto.Factory.FromNotificationWithPrefferedImportancy(notification, userId);

            await _hubContext.Clients.Client(connectionId)
                .SendAsync(methodTag, notificationDto);
        }
    }
}