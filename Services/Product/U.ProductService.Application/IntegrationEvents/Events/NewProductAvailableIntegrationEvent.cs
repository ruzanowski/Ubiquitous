using System;
using U.EventBus.Events;

namespace U.ProductService.Application.IntegrationEvents.Events
{
    public class NewProductAvailableIntegrationEvent: IntegrationEvent
    {
        public Guid ProductId { get; }
        public string Manufacturer { get; }

        public NewProductAvailableIntegrationEvent(Guid productId, string manufacturer)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
        }
    }
}