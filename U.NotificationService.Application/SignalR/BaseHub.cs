using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.NotificationService.Application.Models;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

// ReSharper disable UnusedMember.Global

namespace U.NotificationService.Application.SignalR
{
    public class BaseHub : Hub
    {

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

        public async Task ConfirmReadNotification(Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x=>x.Confirmations)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set to state 'Read'");
                return;
            }

            notification.ChangeStateToRead(new Guid()); //todo: userid

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotification(Guid notifcationId)
        {
            // todo: is user an admin

            var notification = await _context.Notifications
                .Include(x=>x.Confirmations)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set to state 'Delete'");
                return;
            }

            _context.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task HideNotification(Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x=>x.Confirmations)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set to state 'Read'");
                return;
            }

            notification.ChangeStateToHidden(new Guid()); //todo: userid

            await _context.SaveChangesAsync();
        }

        public async Task ChangeNotificationImportancy(Guid notifcationId, Importancy importancy)
        {
            var notification = await _context.Notifications
                .Include(x=>x.Confirmations)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set importancy");
                return;
            }

            notification.SetImportancy(importancy);

            await _context.SaveChangesAsync();
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
//                    x.User.Equals(userId) && // todo: userId
                                           (x.ConfirmationType ==
                                            ConfirmationType.Unread ||
                                            x.ConfirmationType ==
                                            ConfirmationType.Read))
                ).OrderByDescending(x=>(int)x.Importancy)
                .ThenByDescending(x => x.CreationDate)
                .Skip(0)
                .Take(30) // todo: preferences of welcome messages?
                .ToListAsync();
    }
}