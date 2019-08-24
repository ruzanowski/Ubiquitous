using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator, IProductRepository productRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
            var product = await _productRepository.GetByAlternativeIdAsync(@event.GetUniqueId);

            if (product is null)
            {
                var dimensions = new DimensionsDto(@event.Length, @event.Width, @event.Height, @event.Weight);
                var create = new CreateProductCommand(@event.Name, @event.GetUniqueId, @event.PriceInTax, @event.Description, dimensions);

                await _mediator.Send(create);
            }
            else
            {
                var dimensions = new DimensionsDto(@event.Length, @event.Width, @event.Height, @event.Weight);
                var update = new UpdateProductCommand(product.Id, @event.Name,@event.PriceInTax, @event.Description, dimensions);

                await _mediator.Send(update);
            }
        }
    }
}