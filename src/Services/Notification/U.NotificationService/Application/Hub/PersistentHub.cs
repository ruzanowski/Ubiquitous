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
            var notification = new Notification(@event);

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All
                .SendAsync(methodTag, new NotificationDto(notification.Id, @event, notification.IntegrationEventType));
        }

        public async Task SaveAndSendAsync(string methodTag, string who, IntegrationEvent @event)
        {
            var notification = new Notification(@event);

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Client(who)
                .SendAsync(methodTag, new NotificationDto(notification.Id, @event, notification.IntegrationEventType));

        }


    }
}