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
    public class NewProductFetchedCarrierIntegrationEventHandler : ICarrierIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public NewProductFetchedCarrierIntegrationEventHandler(IMediator mediator, IProductRepository productRepository,
            IManufacturerRepository manufacturerRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task Handle(Carrier<NewProductFetchedIntegrationEvent> carrier)
        {
                var product = await _productRepository.GetByAlternativeIdAsync(carrier.IntegrationEventPayload.GetUniqueId);

                if (product is null)
                {
                    var dimensions = new DimensionsDto(carrier.IntegrationEventPayload.Length, carrier.IntegrationEventPayload.Width, carrier.IntegrationEventPayload.Height, carrier.IntegrationEventPayload.Weight);

                    var manufacturer =
                        await _manufacturerRepository.GetUniqueClientIdAsync(carrier.IntegrationEventPayload.ManufacturerId.ToString()) ??
                        Manufacturer.GetDraftManufacturer();

                    var create = new CreateProductCommand(carrier.IntegrationEventPayload.Name, carrier.IntegrationEventPayload.GetUniqueId, carrier.IntegrationEventPayload.PriceInTax,
                        carrier.IntegrationEventPayload.Description, dimensions, manufacturer.Id);

                    await _mediator.Send(create);
                }
                else
                {
                    var dimensions = new DimensionsDto(carrier.IntegrationEventPayload.Length, carrier.IntegrationEventPayload.Width, carrier.IntegrationEventPayload.Height, carrier.IntegrationEventPayload.Weight);
                    var update = new UpdateProductCommand(product.Id, carrier.IntegrationEventPayload.Name, carrier.IntegrationEventPayload.PriceInTax,
                        carrier.IntegrationEventPayload.Description, dimensions);

                    await _mediator.Send(update);
                }
        }
    }
}