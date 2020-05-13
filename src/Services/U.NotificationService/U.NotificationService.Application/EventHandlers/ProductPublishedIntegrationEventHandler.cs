using System.Threading.Tasks;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Application.Builders.PendingNotifications;
using U.NotificationService.Application.SignalR;

namespace U.NotificationService.Application.EventHandlers
{
    public class ProductPublishedSignalRIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedSignalRIntegrationEvent>
    {
        private readonly IPendingEventsService _pendingEventsService;

        public ProductPublishedSignalRIntegrationEventHandler(IPendingEventsService pendingEventsService)
        {
            _pendingEventsService = pendingEventsService;
        }

        public async Task Handle(ProductPublishedSignalRIntegrationEvent @event)
        {
            _pendingEventsService.Add(@event);
            await Task.CompletedTask;
        }
    }
}