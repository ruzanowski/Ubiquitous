using System;
using System.Linq;
using U.NotificationService.Application.Models;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Services.QueryBuilder
{
    public interface INotificationQueryBuilder
    {
        INotificationQueryBuilder WithQueryAndUser(IQueryable<Notification> query, Guid userId);
        INotificationQueryBuilder FilterByConfirmationType(ConfirmationTypePreferences confirmationTypePreferences);
        INotificationQueryBuilder FilterByMinimalImportancy(Importancy minimumLevel);
        INotificationQueryBuilder OrderByCreationDate(bool @descending);
        INotificationQueryBuilder ThenOrderByImportancy(bool @descending);
        INotificationQueryBuilder Take(int numberOfWelcomeMessages);
        IQueryable<Notification> Build();
    }
}