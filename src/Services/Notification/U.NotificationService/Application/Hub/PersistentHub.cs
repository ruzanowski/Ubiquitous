using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using U.EventBus.Events;
using U.NotificationService.Domain;
using U.NotificationService.Infrastracture.Contexts;

namespace U.NotificationService.Application.Hub
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


        public async Task SaveAndSendToAllAsync(string methodTag, IntegrationEvent @event)
        {
            var notificationDto = await SaveNotificationAsync(@event);

            await _hubContext.Clients.All
                .SendAsync(methodTag, notificationDto);
        }

        public async Task SaveAndSendAsync(string methodTag, string who, IntegrationEvent @event)
        {
            var notificationDto = await SaveNotificationAsync(@event);

            await _hubContext.Clients.Client(who)
                .SendAsync(methodTag, notificationDto);

        }

        private async Task<NotificationDto> SaveNotificationAsync(IntegrationEvent @event)
        {
            var notification = new Notification(@event);

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            return new NotificationDto(notification.Id,
                @event,
                notification.IntegrationEventType,
                ConfirmationType.Unread);
        }


    }
}