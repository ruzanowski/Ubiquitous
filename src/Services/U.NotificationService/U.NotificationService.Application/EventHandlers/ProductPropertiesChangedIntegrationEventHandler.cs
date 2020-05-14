using System.Threading.Tasks;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.Builders.PendingNotifications;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductPropertiesChangedSignalRIntegrationEventHandler : IIntegrationEventHandler<ProductPropertiesChangedSignalRIntegrationEvent>
    {
        private readonly IPendingEventsService _pendingEventsService;

        public ProductPropertiesChangedSignalRIntegrationEventHandler(IPendingEventsService pendingEventsService)
        {
            _pendingEventsService = pendingEventsService;
        }

        public async Task Handle(ProductPropertiesChangedSignalRIntegrationEvent @event)
        {
            _pendingEventsService.Add(@event);
            await Task.CompletedTask;
        }
    }
}