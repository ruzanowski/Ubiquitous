using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using U.NotificationService.Application.Models;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Services.QueryBuilder
{
    public class NotificationQueryBuilder : INotificationQueryBuilder
    {
        private IQueryable<Notification> _query;
        private Guid _userId;

        public INotificationQueryBuilder WithQueryAndUser(IQueryable<Notification> query, Guid userId)
        {
            _query = query;
            _userId = userId;
            return this;
        }

        public INotificationQueryBuilder FilterByConfirmationType(ConfirmationTypePreferences confirmationTypePreferences)
        {
            if (confirmationTypePreferences.SeeRead && confirmationTypePreferences.SeeUnread)
            {
                _query = _query.Where(NotAcquiredOrAnyFromReadOrUnread(_userId));
            }
            else if (confirmationTypePreferences.SeeRead)
            {
                _query = _query.Where(NotAcquiredOrAnyFromRead(_userId));
            }
            else if(confirmationTypePreferences.SeeUnread)
            {
                _query = _query.Where(NotAcquiredOrAnyFromUnread(_userId));
            }

            return this;
        }

        public INotificationQueryBuilder FilterByMinimalImportancy(Importancy minimumLevel)
        {
            _query = _query.Where(x => !x.Importancies.Any(y => y.UserId.Equals(_userId)) ||
                                       x.Importancies.Any(y => y.UserId.Equals(_userId)) &&
                                       x.Importancies.First(y => y.UserId.Equals(_userId)).Importancy <= minimumLevel);
            return this;
        }

        private Expression<Func<Notification, bool>> NotAcquiredOrAnyFromReadOrUnread(Guid userId) =>
            notification => !notification.Confirmations.Any() ||
                            notification.Confirmations.Any(x =>
                                x.User.Equals(userId) &&
                                (x.ConfirmationType == ConfirmationType.Unread ||
                                 x.ConfirmationType == ConfirmationType.Read));

        private Expression<Func<Notification, bool>> NotAcquiredOrAnyFromRead(Guid userId) =>
            notification => !notification.Confirmations.Any() ||
                            notification.Confirmations.Any(x =>
                                x.User.Equals(userId) &&
                                x.ConfirmationType == ConfirmationType.Read);

        private Expression<Func<Notification, bool>> NotAcquiredOrAnyFromUnread(Guid userId) =>
            notification => !notification.Confirmations.Any() ||
                            notification.Confirmations.Any(x =>
                                x.User.Equals(userId) &&
                                x.ConfirmationType == ConfirmationType.Unread);

        public INotificationQueryBuilder OrderByCreationDate(bool descending)
        {
            _query = descending
                ? _query.OrderByDescending(x => x.CreationDate)
                : _query.OrderBy(x => x.CreationDate);

            return this;
        }

        public INotificationQueryBuilder ThenOrderByState()
        {
            _query = _query.OrderBy(x => (int) x.Confirmations.FirstOrDefault(y => y.User.Equals(_userId)).ConfirmationType);
            return this;
        }

        public INotificationQueryBuilder ThenOrderByImportancy(bool descending)
        {
            _query = descending
                ? _query.OrderByDescending(x => (int) x.Importancies.FirstOrDefault(z=>z.UserId.Equals(_userId)).Importancy)
                : _query.OrderBy(x =>(int) x.Importancies.FirstOrDefault(z=>z.UserId.Equals(_userId)).Importancy);

            return this;
        }

        public INotificationQueryBuilder Take(int numberOfWelcomeMessages)
        {
            _query = _query.Take(numberOfWelcomeMessages);

            return this;
        }

        public IQueryable<Notification> Build()
        {
            return _query;
        }
    }
}