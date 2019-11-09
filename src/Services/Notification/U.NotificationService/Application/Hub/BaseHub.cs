using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.NotificationService.Application.ConnectionMapping;
using U.NotificationService.Domain;
using U.NotificationService.Infrastracture.Contexts;

namespace U.NotificationService.Application.Hub
{
    public class BaseHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public static readonly ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();

        private readonly NotificationContext _context;
        private readonly ILogger<BaseHub> _logger;

        public BaseHub(NotificationContext context, ILogger<BaseHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
//            var fakeUserGuid = new Guid();
            await LoadAndPushWelcomeMessages(Context.ConnectionId);
            await Clients.All.SendAsync("connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Others.SendAsync("disconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        public async Task MessageReceived(Guid notifcationId)
        {
        }

        public async Task DeleteMessage(Guid notifcationId)
        {
        }

        public async Task HideMessage(Guid notifcationId)
        {
        }

        private async Task LoadAndPushWelcomeMessages(string userId)
        {
            var notifications = await LoadWelcomeMessages(userId);

            foreach (var notification in notifications)
            {
                var welcomeNotification = NotifactionFactory.FromStoredNotification(notification);

                await Clients.Client(userId).SendAsync("WelcomeNotifications", welcomeNotification);
                _logger.LogDebug($"Sent historic notification: '{notification.Id}' to userId: '{userId}'.");
            }
        }

        private async Task<List<Notification>> LoadWelcomeMessages(string userId) =>
            await _context.Notifications
                .Include(x => x.Confirmations)
                .Where(notification => !notification.Confirmations.Any() ||
                                       notification.Confirmations.Any(x =>
//                    x.User.Equals(userId) && //turned off for now for testing
                                           (x.ConfirmationType ==
                                            ConfirmationType.Unread ||
                                            x.ConfirmationType ==
                                            ConfirmationType.Read))
                ).OrderByDescending(x => x.CreationDate)
                .Skip(0)
                .Take(30)
                .ToListAsync();
    }
}