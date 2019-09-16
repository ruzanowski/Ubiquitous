using System;
using U.EventBus.Events;

namespace U.Notification.SignalR.IntegrationEvents.ProductPublished
{
    public class ProductPublishedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }
        
        public string Name { get; }
        
        public decimal Price { get; }
        
        public Guid Manufacturer { get; }

        public ProductPublishedIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}