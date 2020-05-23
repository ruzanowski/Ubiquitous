using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Events.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator,
            IProductRepository productRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
            var product = await _productRepository.GetByAbsoluteComparerAsync(
                @event.ExternalSourceName,
                @event.Id);

            if (product is null)
            {
                var create = new CreateProductCommand(@event.Name,
                    @event.BarCode,
                    @event.Price,
                    @event.Description,
                    new DimensionsDto
                    {
                        Length = @event.Length,
                        Width = @event.Width,
                        Height = @event.Height,
                        Weight = @event.Weight
                    },
                    new ExternalCreation
                    {
                        DuplicationValidated = true,
                        SourceName = @event.ExternalSourceName,
                        SourceId = @event.Id
                    });

                await _mediator.Send(create);
            }
            else
            {
                var update = new UpdateProductCommand(
                    product.Id,
                    @event.Name,
                    @event.Price,
                    @event.Description,
                    new DimensionsDto
                    {
                        Length = @event.Length,
                        Width = @event.Width,
                        Height = @event.Height,
                        Weight = @event.Weight
                    }
                );

                await _mediator.Send(update);
            }
        }
    }
}