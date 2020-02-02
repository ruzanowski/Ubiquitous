using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.Subscription;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.SignalR.Services.Clients;
using U.NotificationService.Application.SignalR.Services.QueryBuilder;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.SignalR.Services.WelcomeNotifications
{
    public class WelcomeNotificationsService : IWelcomeNotificationsService
    {
        private readonly NotificationContext _context;
        private readonly INotificationQueryBuilder _queryBuilder;
        private readonly ISubscriptionService _subscriptionService;


        public WelcomeNotificationsService(NotificationContext context,
            INotificationQueryBuilder queryBuilder,
            ISubscriptionService subscriptionService)
        {
            _context = context;
            _queryBuilder = queryBuilder;
            _subscriptionService = subscriptionService;
        }

        public async Task<List<Notification>> LoadWelcomeMessages(Preferences preferences, Guid userId)
        {
            var orderByCreationTimeDescending = preferences.OrderByCreationTimeDescending;
            var thenOrderByImportancyDescending = preferences.OrderByImportancyDescending;
            var numberOfWelcomeMessages = preferences.NumberOfWelcomeMessages;
            var importancy = preferences.MinimalImportancyLevel;

            var confirmationType = new ConfirmationTypePreferences
            {
                SeeRead = preferences.SeeReadNotifications,
                SeeUnread = preferences.SeeUnreadNotifications
            };

            var notificationsQuery = _queryBuilder
                .WithQueriesAndUser(GetNotificationQuery(),  userId)
                .FilterByConfirmationType(confirmationType)
                .FilterByMinimalImportancy(importancy)
                .OrderByCreationDate(orderByCreationTimeDescending)
//                .ThenOrderByState()
//                .ThenOrderByImportancy(thenOrderByImportancyDescending)
                .Take(numberOfWelcomeMessages)
                .Build();

            await notificationsQuery.ForEachAsync(notification => notification.IncrementProcessedTimes());
            await _context.SaveChangesAsync();

            var notifications = notificationsQuery.ToList();

            return notifications;
        }

        private IQueryable<Notification> GetNotificationQuery() =>
            _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x => x.Importancies);

    }
}