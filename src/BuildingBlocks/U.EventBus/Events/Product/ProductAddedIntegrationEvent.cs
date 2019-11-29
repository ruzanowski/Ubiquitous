using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Product
{
    public class ProductAddedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }

        public string Name { get; }

        public decimal Price { get; }

        public Guid Manufacturer { get; }

        [JsonConstructor]
        public ProductAddedIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}