using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.EventBus.Events.Product;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Domain.Entities.Product.Events;

namespace U.ProductService.Application.Events.DomainEventHandlers
{
    public class ProductPropertiesChangedEventHandler : INotificationHandler<ProductPropertiesChangedDomainEvent>
    {
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductPropertiesChangedEventHandler(IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService;
        }

        public async Task Handle(ProductPropertiesChangedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Additional logic for product domain event handler e.g. validation, publish restriction.
            //event for e.g. SignalR
            var iEvent = new ProductPropertiesChangedIntegrationEvent(@event.ProductId,
                @event.Manufacturer,
                @event.Name,
                @event.Price,
                @event.Description,
                @event.Dimensions.Height,
                @event.Dimensions.Width,
                @event.Dimensions.Length,
                @event.Dimensions.Weight);

            await _productIntegrationEventService.AddAndSaveEventAsync(iEvent);
       }
    }
}