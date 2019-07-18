using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.IntegrationEvents;
using U.ProductService.Application.IntegrationEvents.Events;
using U.ProductService.Domain.Events;

namespace U.ProductService.Application.DomainEventHandlers
{
    public class ProductAddedDomainEventHandler : INotificationHandler<ProductAddedDomainEvent>
    {
        private readonly ILogger<ProductAddedDomainEventHandler>_logger;
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductAddedDomainEventHandler(ILogger<ProductAddedDomainEventHandler> logger, IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService ?? throw new ArgumentNullException(nameof(productIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ProductAddedDomainEvent productAddedEvent, CancellationToken cancellationToken)
        {            
            //getting manufacturer from domain by repository 
            //-- mock --
            var mockManufacturer = $"Manufacturer No. {Guid.NewGuid()}";


            var newProductEvent = new NewProductAvailableIntegrationEvent(productAddedEvent.ProductId, mockManufacturer);
            await _productIntegrationEventService.AddAndSaveEventAsync(newProductEvent);

            _logger.LogInformation(
                $"--- Domain event handled for '{nameof(productAddedEvent)}' " +
                $"with id: '{productAddedEvent.ProductId}' from {mockManufacturer}");
        }
    }
}
