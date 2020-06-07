using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.EventBus.Events.Product;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Domain.Entities.Product.Events;

namespace U.ProductService.Application.Events.DomainEventHandlers
{
    public class ProductPublishedDomainEventHandler : INotificationHandler<ProductPublishedDomainEvent>
    {
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductPublishedDomainEventHandler(IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService;
        }

        public async Task Handle(ProductPublishedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Additional logic for product domain event handler e.g. validation, publish restriction.
            //event for e.g. SignalR
            var iEvent = new ProductPublishedIntegrationEvent(@event.ProductId,
                @event.Name,
                @event.Price,
                @event.Manufacturer);

            await _productIntegrationEventService.AddAndSaveEventAsync(iEvent);
        }
    }
}