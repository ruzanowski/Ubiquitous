using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.Common.Jwt;
using U.Common.Jwt.Claims;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.Services.Subscription;
using U.NotificationService.Application.Services.Users;
using U.NotificationService.Application.Services.WelcomeNotifications;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

// ReSharper disable UnusedMember.Global

namespace U.NotificationService.Application.SignalR
{
    [JwtAuth]
    public class BaseHub : Hub
    {
        private readonly NotificationContext _context;
        private readonly ILogger<BaseHub> _logger;
        private readonly IWelcomeNotificationsService _welcomeNotificationsService;
        private readonly ISubscriptionService _subscriptionService;

        public BaseHub(NotificationContext context,
         ILogger<BaseHub> logger,
            IWelcomeNotificationsService welcomeNotificationsService,
            ISubscriptionService subscriptionService)
        {
            _context = context;
            _logger = logger;
            _welcomeNotificationsService = welcomeNotificationsService;
            _subscriptionService = subscriptionService;
        }

        public UserDto GetCurrentUser() => Context.GetUser();

        public override async Task OnConnectedAsync()
        {
            UserDto currentUser = GetCurrentUser();
            if (Context.IsAuthenticated())
            {
                _logger.LogInformation($"User: '{currentUser.Nickname}' has connected");
            }
            else
            {
                Context.Abort();
            }

            await _subscriptionService.BindConnectionToUserAsync(currentUser.Id, Context.ConnectionId);
            await LoadAndPushWelcomeMessages(currentUser.Id, currentUser.Nickname);

            var preferences = await _subscriptionService.GetMyPreferencesAsync();

            if (!preferences.DoNotNotifyAnyoneAboutMyActivity)
            {
                var userConnected = NotificationDto.NotifactionFactory.UserConnected(currentUser);
                await Clients.All.SendAsync("connected", userConnected);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            UserDto currentUser = Context.GetUser();
            if (Context.IsAuthenticated())
            {
                _logger.LogDebug($"User: '{currentUser.Nickname}' has disconnected");
            }
            else
            {
                Context.Abort();
            }

            await _subscriptionService.UnbindConnectionToUserAsync(currentUser.Id, Context.ConnectionId);

            var preferences = await _subscriptionService.GetMyPreferencesAsync();

            if (!preferences.DoNotNotifyAnyoneAboutMyActivity)
            {
                var userDisonnected = NotificationDto.NotifactionFactory.UserDisconnected(currentUser);
                await Clients.Others.SendAsync("disconnected", userDisonnected);
            }

            await base.OnDisconnectedAsync(ex);
        }

        public async Task ConfirmReadNotification(Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x=>x.Importancies)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set to state 'Read'");
                return;
            }

            var currentUser = GetCurrentUser().Id;

            notification.ChangeStateToRead(currentUser);
            notification.SetImportancy(currentUser, Importancy.Trivial);

            await _context.SaveChangesAsync();
        }

        [JwtAuth("admin")]
        public async Task DeleteNotification(Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x=>x.Importancies)
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
                .Include(x=>x.Importancies)
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
                .Include(x=>x.Importancies)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set importancy");
                return;
            }

            notification.SetImportancy(GetCurrentUser().Id, importancy);

            await _context.SaveChangesAsync();
        }

        private async Task LoadAndPushWelcomeMessages(Guid userId, string userNickname)
        {
            var notifications = await _welcomeNotificationsService.LoadWelcomeMessages(userId);

            foreach (var notification in notifications)
            {
                var welcomeNotification = NotificationDto.NotifactionFactory.FromNotificationWithPrefferedImportancy(notification, GetCurrentUser().Id);

                await Clients.Client(Context.ConnectionId).SendAsync("WelcomeNotifications", welcomeNotification);
                _logger.LogDebug($"Sent historic notification: '{notification.Id}' to '{userNickname}' of id: '{userId}'.");
            }
        }
    }
}