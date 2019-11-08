using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Application.Events.IntegrationEvents.EventHandling;
using U.ProductService.Application.Events.IntegrationEvents.Events;
using U.ProductService.Domain.Events;

namespace U.ProductService.Application.Events.DomainEventHandlers
{
    public class ProductAddedDomainEventHandler : INotificationHandler<ProductAddedDomainEvent>
    {
        private readonly ILogger<ProductAddedDomainEventHandler> _logger;
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductAddedDomainEventHandler(ILogger<ProductAddedDomainEventHandler> logger,
            IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService ??
                                              throw new ArgumentNullException(nameof(productIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ProductAddedDomainEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                $"--- Domain event handled for '{nameof(ProductAddedDomainEvent)}' with id: '{@event.ProductId}'");

            //Additional logic for product domain event handler e.g. validation, publish restriction.
            //event for e.g. SignalR
            var iEvent =
                new ProductAddedIntegrationEvent(@event.ProductId, @event.Name, @event.Price, @event.Manufacturer);
            await _productIntegrationEventService.AddAndSaveEventAsync(iEvent);

            _logger.LogDebug($"--- Integration event published: '{nameof(ProductAddedIntegrationEvent)}");
        }
    }
}