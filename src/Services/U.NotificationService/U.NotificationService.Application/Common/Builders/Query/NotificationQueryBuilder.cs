using System;
using System.Linq;
using System.Linq.Expressions;
using U.Common.Subscription;
using U.NotificationService.Application.Common.Models;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Common.Builders.Query
{
    public class NotificationQueryBuilder : INotificationQueryBuilder
    {
        private IQueryable<Notification> _query;
        private IQueryable<Notification> _pureQuery;
        private Guid _userId;

        public INotificationQueryBuilder WithQueriesAndUser(
            IQueryable<Notification> notifications,
            Guid userId)
        {
            _pureQuery = notifications;
            _query = notifications;
            _userId = userId;
            return this;
        }

        public INotificationQueryBuilder FilterByConfirmationType(
            ConfirmationTypePreferences confirmationTypePreferences)
        {
            if (confirmationTypePreferences.SeeRead && confirmationTypePreferences.SeeUnread)
            {
                _query = _query.Where(NotAcquiredOrAnyFromReadOrUnread(_userId));
            }
            else if (confirmationTypePreferences.SeeRead)
            {
                _query = _query.Where(NotAcquiredOrAnyFromRead(_userId));
            }
            else if (confirmationTypePreferences.SeeUnread)
            {
                _query = _query.Where(NotAcquiredOrAnyFromUnread(_userId));
            }

            return this;
        }

        public INotificationQueryBuilder FilterByMinimalImportancy(Importancy minimumLevel)
        {
            var importancy = _pureQuery
                .SelectMany(x => x.Importancies)
                .FirstOrDefault(x => x.UserId.Equals(_userId));

            if (minimumLevel == Importancy.Trivial)
                return this;

            switch (importancy)
            {
                case null:
                    return this;
                default:
                    _query = _query.Where(x => x.Importancies.Single(y => y.Id.Equals(importancy.Id)).Importancy >= importancy.Importancy);
                    return this;
            }
        }

        private Expression<Func<Notification, bool>> NotAcquiredOrAnyFromReadOrUnread(Guid userId)
        {
            return notification => !notification.Confirmations.Any() ||
                                   notification.Confirmations.Any(x =>
                                       x.User.Equals(userId) &&
                                       (x.ConfirmationType == ConfirmationType.Unread ||
                                        x.ConfirmationType == ConfirmationType.Read));
        }

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
            _query = (_query as IOrderedQueryable<Notification>).ThenByDescending(x =>
                (int) x.Confirmations.FirstOrDefault(y => y.User.Equals(_userId)).ConfirmationType);
            return this;
        }

        public INotificationQueryBuilder ThenOrderByImportancy(bool descending)
        {
            var anyImportancies = _query.Select(x => x.Importancies.Where(y => y.UserId.Equals(_userId))).Any();

            if (anyImportancies)
            {
                _query = descending
                    ? (_query as IOrderedQueryable<Notification>).ThenByDescending(x =>
                        (int) x.Importancies.First(z => z.UserId.Equals(_userId)).Importancy)
                    : (_query as IOrderedQueryable<Notification>).ThenBy(x =>
                        (int) x.Importancies.First(z => z.UserId.Equals(_userId)).Importancy);
            }


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