using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.SignalR;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductAddedSignalRIntegrationEventHandler : IIntegrationEventHandler<ProductAddedSignalRIntegrationEvent>
    {
        private readonly ILogger<ProductAddedSignalRIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductAddedSignalRIntegrationEventHandler(ILogger<ProductAddedSignalRIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(ProductAddedSignalRIntegrationEvent @event)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductAddedSignalRIntegrationEvent), @event);

            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductAddedSignalRIntegrationEvent)}' ---");
        }
    }
}