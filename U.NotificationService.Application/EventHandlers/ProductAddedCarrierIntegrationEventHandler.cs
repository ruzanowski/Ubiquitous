using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.SignalR;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductAddedCarrierIntegrationEventHandler : ICarrierIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly ILogger<ProductAddedCarrierIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductAddedCarrierIntegrationEventHandler(ILogger<ProductAddedCarrierIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(Carrier<ProductAddedIntegrationEvent> @event)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductAddedIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductAddedIntegrationEvent)}' ---");
        }
    }
}