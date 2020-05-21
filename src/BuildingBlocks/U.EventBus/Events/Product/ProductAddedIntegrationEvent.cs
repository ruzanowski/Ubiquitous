using System;
using Newtonsoft.Json;
using U.Common.Subscription;

namespace U.EventBus.Events.Product
{
    public class ProductAddedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; set;}

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid Manufacturer { get; set; }

        public ProductAddedIntegrationEvent()
        {

        }

        [JsonConstructor]
        public ProductAddedIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }

        public override string MethodTag => nameof(ProductAddedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.ProductAddedIntegrationEvent;
    }

    public sealed class ProductAddedSignalRIntegrationEvent : ProductAddedIntegrationEvent
    {
        public ProductAddedSignalRIntegrationEvent(Guid productId,
            string name,
            decimal price,
            Guid manufacturer) : base(productId, name, price, manufacturer)
        {
        }
        public override string MethodTag => nameof(ProductAddedSignalRIntegrationEvent);

    }

    public sealed class ProductAddedEmailIntegrationEvent : ProductAddedIntegrationEvent
    {
        public ProductAddedEmailIntegrationEvent(Guid productId, string name, decimal price, Guid manufacturer) : base(
            productId, name, price, manufacturer)
        {
        }
        public override string MethodTag => nameof(ProductAddedEmailIntegrationEvent);

    }
}