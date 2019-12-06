using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using U.EventBus.Events;
using U.NotificationService.Application.Models;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

// ReSharper disable UnusedMember.Global

namespace U.NotificationService.Application.SignalR
{
    public class PersistentHub
    {
        private readonly NotificationContext _context;
        private readonly IHubContext<BaseHub> _hubContext;

        public PersistentHub(NotificationContext context, IHubContext<BaseHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveAndSendToAllAsync<T>(string methodTag, T @event) where T : IntegrationEvent
        {
            var notification = new Notification(@event);

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            var notificationDto = NotificationDto.NotifactionFactory.GlobalVolatileNotification(notification, @event);

            await _hubContext.Clients.All
                .SendAsync(methodTag, notificationDto);
        }

        public async Task SaveAndSendAsync<T>(string methodTag, string connectionId, Guid userId, T @event)
            where T : IntegrationEvent
        {
            var notification = new Notification(@event);

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            var notificationDto = NotificationDto.NotifactionFactory.FromNotificationWithPrefferedImportancy(notification, userId);

            await _hubContext.Clients.Client(connectionId)
                .SendAsync(methodTag, notificationDto);
        }

    }
}