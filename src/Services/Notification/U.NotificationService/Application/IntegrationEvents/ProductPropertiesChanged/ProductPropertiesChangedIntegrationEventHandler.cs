using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.NotificationService.Application.Hub;
using U.NotificationService.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Application.IntegrationEvents.ProductPropertiesChanged
{
    public class ProductPropertiesChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPropertiesChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedIntegrationEventHandler> _logger;
        private readonly PersistentHub _persistentHubContext;

        public ProductPropertiesChangedIntegrationEventHandler(ILogger<ProductPublishedIntegrationEventHandler> logger,
            PersistentHub persistentHubContext)
        {
            _logger = logger;
            _persistentHubContext = persistentHubContext;
        }

        public async Task Handle(ProductPropertiesChangedIntegrationEvent @event)
        {
            await _persistentHubContext.SaveAndSendToAllAsync(nameof(ProductPropertiesChangedIntegrationEvent), @event);
            
            _logger.LogInformation($"--- Pushed by SignalR: '{nameof(ProductPropertiesChangedIntegrationEvent)}' ---");
        }
    }
}