using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;

namespace U.NotificationService.Application.IntegrationEvents.ProductPublished
{
    public class ProductPublishedCarrierIntegrationEventHandler : ICarrierIntegrationEventHandler<ProductPublishedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedCarrierIntegrationEventHandler> _logger;

        public ProductPublishedCarrierIntegrationEventHandler(ILogger<ProductPublishedCarrierIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(Carrier<ProductPublishedIntegrationEvent> carrier)
        {
//            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPublishedIntegrationEvent), carrier);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPublishedIntegrationEvent)} ---");
        }
    }
}