using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Application.Hub;

namespace U.NotificationService.IntegrationEvents.ProductPublished
{
    public class ProductPublishedIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;
        private readonly UbiquitousHub _ubiquitousHubContext;

        public ProductPublishedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger,
            UbiquitousHub ubiquitousHubContext)
        {
            _logger = logger;
            _ubiquitousHubContext = ubiquitousHubContext;
        }

        public async Task Handle(ProductPublishedIntegrationEvent @event)
        {
            await _ubiquitousHubContext.SaveAndSendToAllAsync(nameof(ProductPublishedIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPublishedIntegrationEvent)} ---");
        }
    }
}