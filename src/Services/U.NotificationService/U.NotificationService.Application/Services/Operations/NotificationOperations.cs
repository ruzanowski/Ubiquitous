using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.Common.Auth;
using U.Common.Subscription;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.Services.Operations
{
    public class NotificationOperations : INotificationsService
    {
        private readonly NotificationContext _context;
        private readonly ILogger<NotificationOperations> _logger;

        public NotificationOperations(NotificationContext context, ILogger<NotificationOperations> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task HideNotification(UserDto currentUser, Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x => x.Importancies)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set to state 'Read'");
                return;
            }

            notification.ChangeStateToHidden(currentUser.Id);
            notification.IncrementProcessedTimes();

            await _context.SaveChangesAsync();
        }

        public async Task ChangeNotificationImportancy(UserDto currentUser, Guid notifcationId, Importancy importancy)
        {
            var notification = await _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x => x.Importancies)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set importancy");
                return;
            }

            notification.SetImportancy(currentUser.Id, importancy);
            notification.IncrementProcessedTimes();

            await _context.SaveChangesAsync();
        }

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

        public async Task ConfirmReadNotification(UserDto currentUser, Guid notifcationId)
        {
            var notification = await _context.Notifications
                .Include(x => x.Confirmations)
                .Include(x => x.Importancies)
                .FirstOrDefaultAsync(x => x.Id.Equals(notifcationId));

            if (notification is null)
            {
                _logger.LogInformation($"{notifcationId} does not exist and cannot set to state 'Read'");
                return;
            }

            notification.ChangeStateToRead(currentUser.Id);
            notification.SetImportancy(currentUser.Id, Importancy.Trivial);
            notification.IncrementProcessedTimes();


            await _context.SaveChangesAsync();
        }


    }
}