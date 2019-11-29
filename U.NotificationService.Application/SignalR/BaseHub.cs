using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.Services;
using U.NotificationService.Application.Services.Preferences;
using U.NotificationService.Application.Services.Subscription;
using U.NotificationService.Application.Services.Users;
using U.NotificationService.Application.Services.WelcomeNotifications;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

// ReSharper disable UnusedMember.Global

namespace U.NotificationService.Application.SignalR
{
    public class BaseHub : Hub
    {
        private readonly NotificationContext _context;
        private readonly ILogger<BaseHub> _logger;
        private readonly IUsersService _usersService;
        private readonly IWelcomeNotificationsService _welcomeNotificationsService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPreferencesService _preferencesService;

        public BaseHub(NotificationContext context, ILogger<BaseHub> logger,
            IUsersService usersService,
            IWelcomeNotificationsService welcomeNotificationsService,
            ISubscriptionService subscriptionService,
            IPreferencesService preferencesService)
        {
            _context = context;
            _logger = logger;
            _usersService = usersService;
            _welcomeNotificationsService = welcomeNotificationsService;
            _subscriptionService = subscriptionService;
            _preferencesService = preferencesService;
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = await _usersService.GetCurrentUserAsync();
            //await _subscriptionService.BindConnectionToUserAsync(currentUser.Id, Context.ConnectionId);
            await LoadAndPushWelcomeMessages(currentUser.Id);


            var doNotNotify = await _preferencesService.DoNotNotifyAnyoneAboutMyActivity();

            if (!doNotNotify)
            {
                var userConnected = NotifactionFactory.UserConnected(currentUser);
                await Clients.All.SendAsync("connected", userConnected);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var currentUser = await _usersService.GetCurrentUserAsync();
           // await _subscriptionService.UnbindConnectionToUserAsync(currentUser.Id, Context.ConnectionId);

            var doNotNotify = await _preferencesService.DoNotNotifyAnyoneAboutMyActivity();

            if (!doNotNotify)
            {
                var userDisonnected = NotifactionFactory.UserDisconnected(currentUser);
                await Clients.Others.SendAsync("disconnected", userDisonnected);
            }

            await base.OnDisconnectedAsync(ex);
        }

        public async Task ConfirmReadNotification(Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x => x.Confirmations)
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
                .Include(x => x.Confirmations)
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
                .Include(x => x.Confirmations)
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
                .Include(x => x.Confirmations)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set importancy");
                return;
            }

            notification.SetImportancy(importancy);

            await _context.SaveChangesAsync();
        }

        private async Task LoadAndPushWelcomeMessages(Guid userId)
        {
            var notifications = await _welcomeNotificationsService.LoadWelcomeMessages(userId);

            foreach (var notification in notifications)
            {
                var welcomeNotification = NotifactionFactory.FromStoredNotification(notification);

                await Clients.Client(userId.ToString()).SendAsync("WelcomeNotifications", welcomeNotification);
                _logger.LogDebug($"Sent historic notification: '{notification.Id}' to userId: '{userId}'.");
            }
        }
    }
}