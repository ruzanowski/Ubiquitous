using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.Services.QueryBuilder;
using U.NotificationService.Application.Services.Subscription;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.Services.WelcomeNotifications
{
    public class WelcomeNotificationsService : IWelcomeNotificationsService
    {
        private readonly NotificationContext _context;
        private readonly INotificationQueryBuilder _queryBuilder;
        private readonly ISubscriptionService _subscriptionService;


        public WelcomeNotificationsService(NotificationContext context,
            INotificationQueryBuilder queryBuilder, ISubscriptionService subscriptionService)
        {
            _context = context;
            _queryBuilder = queryBuilder;
            _subscriptionService = subscriptionService;
        }

        public async Task<List<Notification>> LoadWelcomeMessages(Guid userId)
        {
            var preferences = await _subscriptionService.GetMyPreferencesAsync();

            var orderByCreationTimeDescending = preferences.OrderByCreationTimeDescending;
            var thenOrderByImportancyDescending = preferences.OrderByImportancyDescending;
            var numberOfWelcomeMessages = preferences.NumberOfWelcomeMessages;
            var importancy = preferences.MinimalImportancyLevel;

            var confirmationType = new ConfirmationTypePreferences
            {
                SeeRead = preferences.SeeReadNotifications,
                SeeUnread = preferences.SeeUnreadNotifications
            };

            var notificationsQuery = _queryBuilder.WithQueryAndUser(GetQuery(), userId)
                .FilterByConfirmationType(confirmationType)
                .FilterByMinimalImportancy(importancy)
                .OrderByCreationDate(orderByCreationTimeDescending)
                .ThenOrderByState()
                .ThenOrderByImportancy(thenOrderByImportancyDescending)
                .Take(numberOfWelcomeMessages)
                .Build();

            var notifications = notificationsQuery.ToList();

            return notifications;
        }

        private IQueryable<Notification> GetQuery() =>
            _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x => x.Importancies);
    }
}