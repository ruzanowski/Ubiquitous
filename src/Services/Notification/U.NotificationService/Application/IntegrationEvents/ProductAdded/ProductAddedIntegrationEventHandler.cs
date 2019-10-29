using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Application.Hub;
using U.NotificationService.IntegrationEvents.ProductAdded;

namespace U.NotificationService.Application.IntegrationEvents.ProductAdded
{
    public class ProductAddedIntegrationEventHandler : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly ILogger<ProductAddedIntegrationEventHandler> _logger;
        private readonly UbiquitousHub _ubiquitousHubContext;

        public ProductAddedIntegrationEventHandler(ILogger<ProductAddedIntegrationEventHandler> logger,
            UbiquitousHub ubiquitousHubContext)
        {
            _logger = logger;
            _ubiquitousHubContext = ubiquitousHubContext;
        }

        public async Task Handle(ProductAddedIntegrationEvent @event)
        {
            await _ubiquitousHubContext.SaveAndSendToAllAsync(nameof(ProductAddedIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductAddedIntegrationEvent)}' ---");
        }
    }
}