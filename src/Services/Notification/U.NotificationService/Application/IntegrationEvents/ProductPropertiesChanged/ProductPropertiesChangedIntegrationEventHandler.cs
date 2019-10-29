using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Application.Hub;
using U.NotificationService.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Application.IntegrationEvents.ProductPropertiesChanged
{
    public class ProductPropertiesChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPropertiesChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;
        private readonly UbiquitousHub _ubiquitousHubContext;

        public ProductPropertiesChangedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger,
            UbiquitousHub ubiquitousHubContext)
        {
            _logger = logger;
            _ubiquitousHubContext = ubiquitousHubContext;
        }

        public async Task Handle(ProductPropertiesChangedIntegrationEvent @event)
        {
            await _ubiquitousHubContext.SaveAndSendToAllAsync(nameof(ProductPropertiesChangedIntegrationEvent), @event);
            
            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPropertiesChangedIntegrationEvent)}' ---");
        }
    }
}