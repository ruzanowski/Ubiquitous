using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Hub;

namespace U.NotificationService.IntegrationEvents.ProductPublished
{
    public class ProductPublishedIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;
        private readonly IHubContext<UbiquitousHub> _ubiquitousHubContext;

        public ProductPublishedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger,
            IHubContext<UbiquitousHub> ubiquitousHubContext)
        {
            _logger = logger;
            _ubiquitousHubContext = ubiquitousHubContext;
        }

        public async Task Handle(ProductPublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"--- Received: {nameof(ProductPublishedIntegrationEvent)} ---");

            await _ubiquitousHubContext.Clients.All.SendAsync("ReceiveMessage", "system",
                $"Product '{@event.ProductId}':{@event.Name} of manufacturer: '{@event.Manufacturer}' has been published.");
        }
    }
}