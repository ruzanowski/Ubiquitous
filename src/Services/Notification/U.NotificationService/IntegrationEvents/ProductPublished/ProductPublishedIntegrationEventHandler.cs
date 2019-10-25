using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
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
            await _ubiquitousHubContext.Clients.All.SendAsync(nameof(ProductPublishedIntegrationEvent), @event);

            _logger.LogInformation(
                $"--- Pushed by SignalR: '{nameof(ProductPublishedIntegrationEvent)} ---");
        }
    }
}