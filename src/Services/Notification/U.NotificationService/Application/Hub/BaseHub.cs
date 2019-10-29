using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using U.Common.Extensions;
using U.EventBus.Events;
using U.NotificationService.Application.ConnectionMapping;
using U.NotificationService.Domain;
using U.NotificationService.Infrastracture.Contexts;

namespace U.NotificationService.Application.Hub
{
    public abstract class BaseHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public static readonly ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();

        public abstract Task<IList<Notification>> LoadWelcomeMessages(Guid userId);

        public override async Task OnConnectedAsync()
        {
            var fakeUserGuid = new Guid();
            await LoadWelcomeMessages(fakeUserGuid);
            await Clients.All.SendAsync("connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Others.SendAsync("disconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        public async Task MessageReceived(Guid eventId)
        {
        }

        public async Task DeleteMessage(Guid eventId)
        {
        }

        public async Task HideMessage(Guid eventId)
        {
        }
    }


    public class UbiquitousHub : BaseHub
    {
        private readonly NotificationContext _context;

        public UbiquitousHub(NotificationContext context)
        {
            _context = context;
        }

        public async Task SaveAndSendToAllAsync(string methodTag, IntegrationEvent @event)
        {
            var notification = new Notification
            {
                CreationDate = DateTime.UtcNow,
                IntegrationEvent = @event
            };

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            await Clients.All
                .SendAsync(methodTag, new
                {
                    IntegrationEvent = @event,
                    NotificationId = notification.Id
                });
        }

        public async Task SaveAndSendAsync(string methodTag, string who, IntegrationEvent @event)
        {
            var notification = new Notification
            {
                CreationDate = DateTime.UtcNow,
                IntegrationEvent = @event
            };

            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

            await Clients.Client(who)
                .SendAsync(methodTag, new
                {
                    IntegrationEvent = @event,
                    NotificationId = notification.Id
                });
        }

        public override async Task<IList<Notification>> LoadWelcomeMessages(Guid userId)
        {
            var notifications = await _context.Notifications.Where(notification =>
                notification.Confirmations.Any(x =>
                    (x.ConfirmationType == ConfirmationType.Unread ||
                     x.ConfirmationType == ConfirmationType.Read) &&
                    x.User.Equals(userId))
            ).ToListAsync();

            foreach (var notification in notifications)
            {
                await Clients.Client(userId.ToString()).SendAsync(notification.IntegrationEvent.GetGenericTypeName(),
                    new
                    {
                        IntegrationEvent = notification.IntegrationEvent,
                        NotificationId = notification.Id
                    });
            }

            return notifications;
        }
    }
}