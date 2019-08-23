using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;

namespace U.Notification.IntegrationEvents.EventHandling
{
    public class ProductPublishedIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;

        public ProductPublishedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ProductPublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"--- Received: {nameof(ProductPublishedIntegrationEvent)} ---");
            await Task.CompletedTask;

        }
    }
}