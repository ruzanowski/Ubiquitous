using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;

namespace U.Notification.SignalR.IntegrationEvents.EventHandling
{
    public class ProductAddedIntegrationEventHandler : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly ILogger<ProductAddedIntegrationEventHandler> _logger;

        public ProductAddedIntegrationEventHandler(ILogger<ProductAddedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ProductAddedIntegrationEvent @event)
        {
            _logger.LogInformation($"--- Received: {nameof(ProductAddedIntegrationEvent)} ---");
            await Task.CompletedTask;

        }
    }
}