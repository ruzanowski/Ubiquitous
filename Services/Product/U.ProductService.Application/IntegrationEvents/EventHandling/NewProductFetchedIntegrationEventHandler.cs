using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.FetchService.Api.IntegrationEvents;
using U.ProductService.Application.Commands;
using U.ProductService.Domain.Aggregates.Product;

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
            var isNew = await _productRepository.AnyAsync(@event.ProductUniqueCode);

            if (!isNew)
            {
                var createProduct = new CreateProductCommand(null, null, null, null);
                await _mediator.Send(createProduct);
            }
            else
            {
                var createProduct = new CreateProductCommand(null, null, null, null);
                await _mediator.Send(createProduct);
            }
        }
    }
}