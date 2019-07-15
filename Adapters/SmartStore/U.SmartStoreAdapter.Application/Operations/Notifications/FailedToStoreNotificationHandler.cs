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
    public class FailedToStoreNotificationHandler : INotificationHandler<FailedToStoreNotification<SmartProductDto, Product>>
    {
        private readonly ILogger<FailedToStoreNotification<SmartProductDto, Product>> _logger;

        public FailedToStoreNotificationHandler(ILogger<FailedToStoreNotification<SmartProductDto, Product>> logger)
        {
            _logger = logger;
        }

        public async Task Handle(FailedToStoreNotification<SmartProductDto, Product> request,
            CancellationToken cancellationToken)
        {
            var requestedIds = string.Join(", ",
                request.DataTransaction.RequestData.ManufacturerId + "." +
                request.DataTransaction.RequestData.ProductUniqueCode);
            var resultIds = string.Join(", ",
                request.DataTransaction.ResultData.SystemName);
            _logger.LogError(
                $"************************\n" +
                $"Failed to Store {request.EntityName}.\n" +
                $"TransactionId: {request.DataTransaction.TransactionId}.\n" +
                $"Requested at: {request.DataTransaction.Requested.ToString(CultureInfo.CurrentCulture)}\n" +
                $"RequestedData: {requestedIds}.\n" +
                $"ResultData: {resultIds}.\n" +
                $"With Error: {request.Error}" +
                $"\nChanges Rolled Back: {(request.DataTransaction.RolledBack ? "true" : "false")}" +
                $"\n************************\n");

            // or any different logic that informs about failed storing. e.g: sms, mails etc.
        }
    }
}