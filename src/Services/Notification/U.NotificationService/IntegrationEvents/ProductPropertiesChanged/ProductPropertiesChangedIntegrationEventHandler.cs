using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Hub;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.IntegrationEvents.ProductPropertiesChanged
{
    public class ProductPropertiesChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPropertiesChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;
        private readonly IHubContext<UbiquitousHub> _ubiquitousHubContext;

        public ProductPropertiesChangedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger,
            IHubContext<UbiquitousHub> ubiquitousHubContext)
        {
            _logger = logger;
            _ubiquitousHubContext = ubiquitousHubContext;
        }

        public async Task Handle(ProductPropertiesChangedIntegrationEvent @event)
        {
            await _ubiquitousHubContext.Clients.All.SendAsync(nameof(ProductPropertiesChangedIntegrationEvent), @event);
            
            _logger.LogInformation(
                $"--- Pushed by SignalR: '{nameof(ProductPropertiesChangedIntegrationEvent)}' ---");
        }
    }
}