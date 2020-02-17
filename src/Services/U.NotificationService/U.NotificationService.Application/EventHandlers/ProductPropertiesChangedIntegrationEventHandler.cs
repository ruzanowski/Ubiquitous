using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.SignalR;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductPropertiesChangedSignalRIntegrationEventHandler : IIntegrationEventHandler<ProductPropertiesChangedSignalRIntegrationEvent>
    {
        private readonly ILogger<ProductPropertiesChangedSignalRIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductPropertiesChangedSignalRIntegrationEventHandler(ILogger<ProductPropertiesChangedSignalRIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(ProductPropertiesChangedSignalRIntegrationEvent @event)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPropertiesChangedSignalRIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPropertiesChangedSignalRIntegrationEvent)}' ---");
        }
    }
}