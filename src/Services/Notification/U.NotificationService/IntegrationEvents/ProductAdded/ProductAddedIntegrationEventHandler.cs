using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Hub;

namespace U.NotificationService.IntegrationEvents.ProductAdded
{
    public class ProductAddedIntegrationEventHandler : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly ILogger<ProductAddedIntegrationEventHandler> _logger;
        private readonly IHubContext<UbiquitousHub> _ubiquitousHubContext;

        public ProductAddedIntegrationEventHandler(ILogger<ProductAddedIntegrationEventHandler> logger,
            IHubContext<UbiquitousHub> ubiquitousHubContext)
        {
            _logger = logger;
            _ubiquitousHubContext = ubiquitousHubContext;
        }

        public async Task Handle(ProductAddedIntegrationEvent @event)
        {
            _logger.LogInformation($"--- Received: {nameof(ProductAddedIntegrationEvent)} ---");

            await _ubiquitousHubContext.Clients.All.SendAsync("ReceiveMessage", "system",
                $"Product '{@event.ProductId}':{@event.Name} of manufacturer: '{@event.Manufacturer}' has been added.");
        }
    }
}