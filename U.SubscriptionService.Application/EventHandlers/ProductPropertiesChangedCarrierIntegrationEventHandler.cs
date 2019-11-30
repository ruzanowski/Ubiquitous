using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Application.IntegrationEvents.ProductPropertiesChanged
{
    public class ProductPropertiesChangedCarrierIntegrationEventHandler : ICarrierIntegrationEventHandler<ProductPropertiesChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedCarrierIntegrationEventHandler> _logger;

        public ProductPropertiesChangedCarrierIntegrationEventHandler(ILogger<ProductPublishedCarrierIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(Carrier<ProductPropertiesChangedIntegrationEvent> carrier)
        {
//            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPropertiesChangedIntegrationEvent), carrier);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPropertiesChangedIntegrationEvent)}' ---");
        }
    }
}