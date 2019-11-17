using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Application.Hub;

namespace U.NotificationService.Application.IntegrationEvents.ProductPublished
{
    public class ProductPublishedIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductPublishedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(ProductPublishedIntegrationEvent @event)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPublishedIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPublishedIntegrationEvent)} ---");
        }
    }
}