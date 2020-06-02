using System.Threading.Tasks;
using U.EventBus.Abstractions;
using U.EventBus.Events.Product;
using U.NotificationService.Application.Services.PendingEvents;

namespace U.NotificationService.Application.EventHandlers
{
    public class
        ProductAddedSignalRIntegrationEventHandler : IIntegrationEventHandler<ProductAddedSignalRIntegrationEvent>
    {
        private readonly IPendingEventsService _pendingEventsService;

        public ProductAddedSignalRIntegrationEventHandler(IPendingEventsService pendingEventsService)
        {
            _pendingEventsService = pendingEventsService;
        }

        public async Task Handle(ProductAddedSignalRIntegrationEvent @event)
        {
            _pendingEventsService.Add(@event);
            await Task.CompletedTask;
        }
    }
}