using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Fetch;
using U.EventBus.Events.Product;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Manufacturer;

namespace U.ProductService.Application.Events.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator, IProductRepository productRepository,
            IManufacturerRepository manufacturerRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
                var product = await _productRepository.GetByAlternativeIdAsync(@event.GetUniqueId);

                if (product is null)
                {
                    var dimensions = new DimensionsDto(@event.Length, @event.Width, @event.Height, @event.Weight);

                    var manufacturer =
                        await _manufacturerRepository.GetUniqueClientIdAsync(@event.ManufacturerId.ToString()) ??
                        Manufacturer.GetDraftManufacturer();

                    var create = new CreateProductCommand(@event.Name, @event.GetUniqueId, @event.PriceInTax,
                        @event.Description, dimensions, manufacturer.Id);

                    await _mediator.Send(create);
                }
                else
                {
                    var dimensions = new DimensionsDto(@event.Length, @event.Width, @event.Height, @event.Weight);
                    var update = new UpdateProductCommand(product.Id, @event.Name, @event.PriceInTax,
                        @event.Description, dimensions);

                    await _mediator.Send(update);
                }
        }
    }
}