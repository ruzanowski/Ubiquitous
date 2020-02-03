using System;
using System.Linq;
using U.Common.Subscription;
using U.NotificationService.Application.Models;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.SignalR.Services.QueryBuilder
{
    public interface INotificationQueryBuilder
    {
        INotificationQueryBuilder WithQueriesAndUser(
            IQueryable<Notification> query,
            Guid userId);

        INotificationQueryBuilder FilterByConfirmationType(ConfirmationTypePreferences confirmationTypePreferences);
        INotificationQueryBuilder FilterByMinimalImportancy(Importancy minimumLevel);
        INotificationQueryBuilder OrderByCreationDate(bool @descending);
        INotificationQueryBuilder ThenOrderByState();

        INotificationQueryBuilder ThenOrderByImportancy(bool @descending);
        INotificationQueryBuilder Take(int numberOfWelcomeMessages);
        IQueryable<Notification> Build();
    }
}