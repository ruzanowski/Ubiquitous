using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.EventBus.Events;
using U.EventBus.Events.Product;
using U.NotificationService.Domain.Entities;
using U.ProductService.Application.Events.IntegrationEvents;
using U.ProductService.Domain.Events;

namespace U.ProductService.Application.Events.DomainEventHandlers
{
    public class ProductPropertiesChangedEventHandler : INotificationHandler<ProductPropertiesChangedDomainEvent>
    {
        private readonly ILogger<ProductPropertiesChangedEventHandler> _logger;
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public ProductPropertiesChangedEventHandler(ILogger<ProductPropertiesChangedEventHandler> logger,
            IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService ??
                                              throw new ArgumentNullException(nameof(productIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ProductPropertiesChangedDomainEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                $"--- Domain event handled for '{nameof(ProductPropertiesChangedDomainEvent)}' with id: '{@event.ProductId}'");

            //Additional logic for product domain event handler e.g. validation, publish restriction.
            //event for e.g. SignalR
            var iEvent = new ProductPropertiesChangedIntegrationEvent(@event.ProductId, @event.Manufacturer, @event.Variances);

            var carrieredEvent = new Carrier<ProductPropertiesChangedIntegrationEvent>
            {
                Importancy = Importancy.Trivial,
                RouteType = RouteType.Primary,
                IntegrationEventPayload = iEvent,
                IntegrationEventType = IntegrationEventType.ProductPublishedIntegrationEvent
            };

            await _productIntegrationEventService.AddAndSaveEventAsync(carrieredEvent);

            _logger.LogDebug($"--- Integration event published: '{nameof(ProductPropertiesChangedIntegrationEvent)}");
        }
    }
}