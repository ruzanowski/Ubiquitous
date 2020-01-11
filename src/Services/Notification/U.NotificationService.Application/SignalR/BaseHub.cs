using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.Common.Jwt.Attributes;
using U.Common.Jwt.Claims;
using U.Common.Subscription;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.SignalR.Services;
using U.NotificationService.Application.SignalR.Services.Clients;
using U.NotificationService.Application.SignalR.Services.Service;
using U.NotificationService.Application.SignalR.Services.WelcomeNotifications;

// ReSharper disable UnusedMember.Global

namespace U.NotificationService.Application.SignalR
{
    [JwtAuth]
    public class BaseHub : Hub
    {
        private readonly ILogger<BaseHub> _logger;
        private readonly IWelcomeNotificationsService _welcomeNotificationsService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly INotificationsService _notificationsService;

        public BaseHub(ILogger<BaseHub> logger,
            IWelcomeNotificationsService welcomeNotificationsService,
            ISubscriptionService subscriptionService,
            INotificationsService notificationsService)
        {
            _logger = logger;
            _welcomeNotificationsService = welcomeNotificationsService;
            _subscriptionService = subscriptionService;
            _notificationsService = notificationsService;
        }

        private UserDto GetCurrentUser() => Context.GetUserOrThrow();

        public override async Task OnConnectedAsync()
        {
            UserDto currentUser = GetUserOrAbort();

            await _subscriptionService.BindConnectionToUserAsync(currentUser.Id, Context.ConnectionId);

            var preferences = await _subscriptionService.GetMyPreferencesAsync(currentUser.Id);
            var welcomeNotifications = await LoadWelcomeMessages(preferences, currentUser.Id);

            foreach (var welcomeNotification in welcomeNotifications)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("WelcomeNotifications", welcomeNotification);
                _logger.LogInformation(
                    $"Sent historic notification: '{welcomeNotification.Id}' to '{currentUser.Nickname}' of id: '{currentUser.Id}'.");
            }

            if (preferences.DoNotNotifyAnyoneAboutMyActivity == false)
            {
                var userConnected = NotificationDto.NotifactionFactory.UserConnected(currentUser);
                await Clients.Others.SendAsync("connected", userConnected);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            UserDto currentUser = GetUserOrAbort();

            var preferences = await _subscriptionService.GetMyPreferencesAsync(currentUser.Id);

            await _subscriptionService.UnbindConnectionToUserAsync(currentUser.Id, Context.ConnectionId);

            if (preferences.DoNotNotifyAnyoneAboutMyActivity == false)
            {
                var userDisonnected = NotificationDto.NotifactionFactory.UserDisconnected(currentUser);
                await Clients.Others.SendAsync("disconnected", userDisonnected);
            }

            await base.OnDisconnectedAsync(ex);
        }

        [JwtAuth]
        public async Task ConfirmReadNotification(Guid notifcationId) =>
            await _notificationsService.ConfirmReadNotification(GetCurrentUser(), notifcationId);

        [JwtAuth("admin")]
        public async Task DeleteNotification(Guid notifcationId) =>
            await _notificationsService.DeleteNotification(notifcationId);

        [JwtAuth]
        public async Task HideNotification(Guid notifcationId) =>
            await _notificationsService.HideNotification(GetCurrentUser(), notifcationId);

        [JwtAuth]
        public async Task ChangeNotificationImportancy(Guid notifcationId, Importancy importancy) =>
            await _notificationsService.ChangeNotificationImportancy(GetCurrentUser(), notifcationId, importancy);

        private async Task<List<NotificationDto>> LoadWelcomeMessages(Preferences preferences, Guid userId)
        {
            var notifications = await _welcomeNotificationsService.LoadWelcomeMessages(preferences, userId);

            return notifications
                .Select(x =>
                    NotificationDto.NotifactionFactory.FromNotificationWithPrefferedImportancy(x, GetCurrentUser().Id))
                .ToList();
        }

        private UserDto GetUserOrAbort()
        {
            if (Context.IsAuthenticated())
                _logger.LogDebug($"User: '{GetCurrentUser().Nickname}' has disconnected");
            else
                Context.Abort();

            return Context.GetUserOrThrow();
        }
    }
}