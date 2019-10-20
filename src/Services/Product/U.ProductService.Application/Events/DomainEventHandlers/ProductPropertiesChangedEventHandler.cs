using System;
using System.Threading;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Application.Events.IntegrationEvents.Events;
using U.ProductService.Domain.Events;

namespace U.ProductService.Application.Events.DomainEventHandlers
{
    public class ProductPropertiesChangedDomainEventHandler : INotificationHandler<ProductPropertiesChangedDomainEvent>
    {
        private readonly ILogger<ProductPublishedDomainEventHandler> _logger;
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductPublishedDomainEventHandler(ILogger<ProductPublishedDomainEventHandler> logger,
            IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService ??
                                              throw new ArgumentNullException(nameof(productIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ProductPublishedDomainEvent @event, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("IntegrationEventContext",
                $"{@event.ProductId}-{typeof(ProductPublishedDomainEventHandler).Namespace}"))
            {
                _logger.LogInformation(
                    $"--- Domain event handled for '{nameof(ProductPublishedDomainEvent)}' with id: '{@event.ProductId}'");

                //Additional logic for product domain event handler e.g. validation, publish restriction.
                //event for e.g. SignalR
                var iEvent = new ProductPublishedIntegrationEvent(@event.ProductId, @event.Name, @event.Price,
                    @event.Manufacturer);
                await _productIntegrationEventService.AddAndSaveEventAsync(iEvent);

                _logger.LogInformation($"--- Integration event published: '{nameof(ProductPublishedIntegrationEvent)}");
            }
        }
    }
}