using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.ProductService.Application.Products.Commands.CreateProduct;
using U.ProductService.Application.Products.Commands.UpdateProduct;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<NewProductFetchedIntegrationEventHandler> _logger;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator, IProductRepository productRepository,
            ILogger<NewProductFetchedIntegrationEventHandler> logger)
        {
            _mediator = mediator;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
            _logger.LogInformation($"--- Received: {nameof(NewProductFetchedIntegrationEvent)} ---");

            var product = await _productRepository.GetByAlternativeIdAsync(@event.GetUniqueId);

            if (product is null)
            {
                _logger.LogInformation(
                    $"--- Product does not exist with alternate key: '{@event.GetUniqueId}' ---");

                var dimensions = new Dimensions(@event.Length, @event.Width, @event.Height, @event.Weight);
                var create = new CreateProductCommand(@event.Name, @event.GetUniqueId, @event.PriceInTax,
                    @event.Description, dimensions);

                _logger.LogInformation($"--- Raised Command: {nameof(CreateProductCommand)} ---");

                await _mediator.Send(create);
            }
            else
            {
                _logger.LogInformation($"--- Product exists with alternate key: '{@event.GetUniqueId}' ---");

                var dimensions = new Dimensions(@event.Length, @event.Width, @event.Height, @event.Weight);
                var update = new UpdateProductCommand(product.Id, @event.Name,@event.PriceInTax, @event.Description, dimensions);

                _logger.LogInformation($"--- Raised Command: {nameof(UpdateProductCommand)} ---");

                await _mediator.Send(update);
            }
        }
    }
}