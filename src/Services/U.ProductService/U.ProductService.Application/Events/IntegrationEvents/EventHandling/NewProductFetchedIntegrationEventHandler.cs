using System;
using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
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
        private readonly ICategoryRepository _categoryRepository;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator,
            IProductRepository productRepository,
            IManufacturerRepository manufacturerRepository,
            ICategoryRepository categoryRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
            _manufacturerRepository = manufacturerRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
            var manufacturer = await SetDefaultRandomManufacturerAsync();

            var product = await _productRepository.GetByAbsoluteComparerAsync(
                @event.ExternalSourceName,
                @event.Id);

            if (product is null)
            {
                var dimensions = new DimensionsDto(
                    @event.Length,
                    @event.Width,
                    @event.Height,
                    @event.Weight);

                var create = new CreateProductCommand(@event.Name,
                    @event.BarCode,
                    @event.Price,
                    @event.Description,
                    dimensions,
                    @event.ExternalSourceName,
                    @event.Id,
                    manufacturer.Id);

                await _mediator.Send(create);
            }
            else
            {
                var dimensions = new DimensionsDto(
                    @event.Length,
                    @event.Width,
                    @event.Height,
                    @event.Weight);

                var update = new UpdateProductCommand(
                    product.Id,
                    @event.Name,
                    @event.Price,
                    @event.Description,
                    dimensions);

                await _mediator.Send(update);
            }
        }

        private async Task<Manufacturer> SetDefaultRandomManufacturerAsync()
        {
            var rnd = new Random();
            var manufacturers = await _manufacturerRepository.GetManyAsync();
            var mod = rnd.Next() % manufacturers.Count;

            return manufacturers[mod];
        }
    }
}