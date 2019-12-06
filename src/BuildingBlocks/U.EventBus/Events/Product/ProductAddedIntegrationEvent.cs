using System;
using Newtonsoft.Json;
using U.EventBus.Events.Notification;

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

    public sealed class ProductAddedSignalRIntegrationEvent : ProductAddedIntegrationEvent
    {
        public ProductAddedSignalRIntegrationEvent(Guid productId,
            string name,
            decimal price,
            Guid manufacturer) : base(productId, name, price, manufacturer)
        {
        }
    }

    public sealed class ProductAddedEmailIntegrationEvent : ProductAddedIntegrationEvent
    {
        public ProductAddedEmailIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer) : base(
            productId, name, price, manufacturer)
        {
        }
    }
}