using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;

namespace U.SubscriptionService.Application.EventHandlers
{
    public class ProductAddedCarrierIntegrationEventHandler : ICarrierIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly ILogger<ProductAddedCarrierIntegrationEventHandler> _logger;

        public ProductAddedCarrierIntegrationEventHandler(ILogger<ProductAddedCarrierIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(Carrier<ProductAddedIntegrationEvent> @event)
        {
//            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductAddedIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductAddedIntegrationEvent)}' ---");
        }
    }
}