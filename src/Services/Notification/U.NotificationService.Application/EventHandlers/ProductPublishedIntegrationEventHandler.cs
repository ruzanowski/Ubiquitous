using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.SignalR;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductPublishedSignalRIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedSignalRIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedSignalRIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductPublishedSignalRIntegrationEventHandler(ILogger<ProductPublishedSignalRIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(ProductPublishedSignalRIntegrationEvent @event)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPublishedSignalRIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPublishedSignalRIntegrationEvent)} ---");
        }
    }
}