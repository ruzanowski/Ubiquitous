using System;
using U.Common.Subscription;

namespace U.EventBus.Events.Product
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
        public override string MethodTag => nameof(ProductPublishedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.ProductPublishedIntegrationEvent;
    }

    public sealed class ProductPublishedSignalRIntegrationEvent : ProductPublishedIntegrationEvent
    {
        public ProductPublishedSignalRIntegrationEvent(Guid productId,
            string name,
            decimal price,
            Guid manufacturer) :
            base(productId, name, price, manufacturer)
        {
        }
        public override string MethodTag => nameof(ProductPublishedSignalRIntegrationEvent);

    }

    public sealed class ProductPublishedEmailIntegrationEvent : ProductPublishedIntegrationEvent
    {
        public ProductPublishedEmailIntegrationEvent(Guid productId,
            string name,
            decimal price,
            Guid manufacturer) :
            base(productId, name, price, manufacturer)
        {
        }
        public override string MethodTag => nameof(ProductPublishedEmailIntegrationEvent);

    }
}