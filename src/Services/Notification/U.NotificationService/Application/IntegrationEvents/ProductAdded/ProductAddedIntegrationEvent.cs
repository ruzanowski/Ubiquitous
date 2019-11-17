using System;
using U.EventBus.Events;

namespace U.NotificationService.Application.IntegrationEvents.ProductAdded
{
    public class ProductAddedIntegrationEvent: IntegrationEvent
    {
        public Guid ProductId { get;  }
        
        public string Name { get; }
        
        public decimal Price { get; }
        
        public Guid Manufacturer { get; }
        public ProductAddedIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}