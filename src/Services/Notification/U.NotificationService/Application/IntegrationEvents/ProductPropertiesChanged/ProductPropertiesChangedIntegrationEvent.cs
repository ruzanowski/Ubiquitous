using System;
using U.EventBus.Events;

namespace U.NotificationService.IntegrationEvents.ProductPropertiesChanged
{
    public class ProductPropertiesChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }

        public string Name { get; }

        public decimal Price { get; }
        public Guid Manufacturer { get; set; }


        public ProductPropertiesChangedIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}