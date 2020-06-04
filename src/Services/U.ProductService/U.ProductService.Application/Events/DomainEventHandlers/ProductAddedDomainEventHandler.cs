using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.EventBus.Events.Product;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Domain.Entities.Product.Events;

namespace U.ProductService.Application.Events.DomainEventHandlers
{
    public class ProductAddedDomainEventHandler : INotificationHandler<ProductAddedDomainEvent>
    {
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductAddedDomainEventHandler(IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService;
        }

        public async Task Handle(ProductAddedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Additional logic for product domain event handler e.g. validation, publish restriction.
            //event for e.g. SignalR
            var integrationEvent = new ProductAddedIntegrationEvent(@event.ProductId, @event.Name, @event.Price, @event.Manufacturer);

            await _productIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}