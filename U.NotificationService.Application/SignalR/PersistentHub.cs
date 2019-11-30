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


        public async Task SaveAndSendToAllAsync<T>(string methodTag, Carrier<T> carrier) where T : IntegrationEvent
        {
            var notificationDto = await SaveNotificationAsync(carrier);

            await _hubContext.Clients.All
                .SendAsync(methodTag, notificationDto);
        }

        public async Task SaveAndSendAsync<T>(string methodTag, string who, Carrier<T> carrier) where T : IntegrationEvent
        {
            var notificationDto = await SaveNotificationAsync(carrier);

            await _hubContext.Clients.Client(who)
                .SendAsync(methodTag, notificationDto);
        }

        private async Task<NotificationDto> SaveNotificationAsync<T>(Carrier<T> carrier) where T : IntegrationEvent
        {
            var notification = new Notification(carrier.IntegrationEventPayload, carrier.Importancy.GetValueOrDefault());

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            return new NotificationDto(notification.Id,
                carrier.IntegrationEventPayload,
                notification.IntegrationEventType,
                ConfirmationType.Unread,
                notification.Importancy);
        }



    }
}