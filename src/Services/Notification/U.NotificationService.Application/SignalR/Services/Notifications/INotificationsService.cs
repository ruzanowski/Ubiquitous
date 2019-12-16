using System;
using System.Threading.Tasks;
using U.Common.Jwt.Claims;
using U.Common.Subscription;

namespace U.NotificationService.Application.SignalR.Services.Notifications
{
    public interface INotificationsService
    {
        Task HideNotification(UserDto currentUser, Guid notifcationId);
        Task ChangeNotificationImportancy(UserDto currentUser, Guid notifcationId, Importancy importancy);
        Task DeleteNotification(Guid notifcationId);
        Task ConfirmReadNotification(UserDto currentUser, Guid notifcationId);
    }
}