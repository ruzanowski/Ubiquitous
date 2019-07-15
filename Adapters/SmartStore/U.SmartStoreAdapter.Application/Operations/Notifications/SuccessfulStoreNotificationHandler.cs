using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.SmartStoreAdapter.Api.Notifications;
using U.SmartStoreAdapter.Api.Products;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

#pragma warning disable 1998

namespace U.SmartStoreAdapter.Application.Operations.Notifications
{
    public class SuccessfulStoreNotificationHandler : INotificationHandler<SuccessfulStoreNotification<SmartProductDto, Product>>
    {
        private readonly ILogger _logger;

        public SuccessfulStoreNotificationHandler(ILogger<SuccessfulStoreNotificationHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(SuccessfulStoreNotification<SmartProductDto, Product> notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"************************\n" +
                $"Succeeded to Store {notification.EntityName}.\n" +
                $"TransactionId: {notification.DataTransaction.TransactionId}.\n" +
                $"Requested at: {notification.DataTransaction.Requested.ToString(CultureInfo.CurrentCulture)}" +
                $"\nAdded: 1 out of 1 requested." +
                $"\nChanges Commited? : {(notification.DataTransaction.RolledBack ? "false" : "true")}" +
                $"\n************************\n");
            // or any different logic that informs about failed storing. e.g: sms, mails etc.
        }
    }
}