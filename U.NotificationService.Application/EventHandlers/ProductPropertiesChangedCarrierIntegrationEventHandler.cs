using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.SignalR;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductPropertiesChangedCarrierIntegrationEventHandler : ICarrierIntegrationEventHandler<ProductPropertiesChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedCarrierIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductPropertiesChangedCarrierIntegrationEventHandler(ILogger<ProductPublishedCarrierIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(Carrier<ProductPropertiesChangedIntegrationEvent> carrier)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPropertiesChangedIntegrationEvent), carrier);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPropertiesChangedIntegrationEvent)}' ---");
        }
    }
}