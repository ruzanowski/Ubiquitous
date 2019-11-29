using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.NotificationService.Application.Models;
using U.NotificationService.Application.Services.Preferences;
using U.NotificationService.Application.Services.QueryBuilder;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.Services.WelcomeNotifications
{
    public class WelcomeNotificationsService : IWelcomeNotificationsService
    {
        private readonly NotificationContext _context;
        private readonly IPreferencesService _preferences;
        private readonly INotificationQueryBuilder _queryBuilder;

        public WelcomeNotificationsService(NotificationContext context,
            IPreferencesService preferences,
            INotificationQueryBuilder queryBuilder)
        {
            _context = context;
            _preferences = preferences;
            _queryBuilder = queryBuilder;
        }

        public async Task<List<Notification>> LoadWelcomeMessages(Guid userId)
        {
            var orderByCreationTimeDescending = await _preferences.OrderByCreationTimeDescending();
            var thenOrderByImportancyDescending = await _preferences.OrderByImportancyDescending();
            var numberOfWelcomeMessages = await _preferences.NumberOfWelcomeMessages();
            var importancy = await _preferences.MinimalImportancyLevel();

            var confirmationType = new ConfirmationTypePreferences
            {
                SeeRead = await _preferences.SeeReadNotifications(),
                SeeUnread = await _preferences.SeeUnreadNotifications()
            };

            var notificationsQuery = _queryBuilder.WithQueryAndUser(GetQuery(), userId)
                .FilterByConfirmationType(confirmationType)
                .FilterByMinimalImportancy(importancy)
                .OrderByCreationDate(orderByCreationTimeDescending)
                .ThenOrderByImportancy(thenOrderByImportancyDescending)
                .Take(numberOfWelcomeMessages)
                .Build();

            return await notificationsQuery.ToListAsync();
        }

        private IQueryable<Notification> GetQuery() =>
            _context.Notifications
            .Include(x => x.Confirmations);
    }
}