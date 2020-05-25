using System;
using System.Collections.Generic;
using U.Common.Subscription;

namespace U.EventBus.Events.Product
{
    public class ProductPropertiesChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }

        public Guid Manufacturer { get; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Weigth { get; set; }

        public ProductPropertiesChangedIntegrationEvent(Guid productId, Guid manufacturer, string name, decimal price,
            string description, decimal height, decimal width, decimal length, decimal weigth)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
            Name = name;
            Price = price;
            Description = description;
            Height = height;
            Width = width;
            Length = length;
            Weigth = weigth;
        }

        public override string MethodTag => nameof(ProductPropertiesChangedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.ProductPropertiesChangedIntegrationEvent;
    }

    public sealed class ProductPropertiesChangedSignalRIntegrationEvent : ProductPropertiesChangedIntegrationEvent
    {
        public override string MethodTag => nameof(ProductPropertiesChangedSignalRIntegrationEvent);

        public ProductPropertiesChangedSignalRIntegrationEvent(Guid productId, Guid manufacturer, string name,
            decimal price, string description, decimal height, decimal width, decimal length, decimal weigth) : base(
            productId, manufacturer, name, price, description, height, width, length, weigth)
        {
        }
    }

    public sealed class ProductPropertiesChangedEmailIntegrationEvent : ProductPropertiesChangedIntegrationEvent
    {
        public override string MethodTag => nameof(ProductPropertiesChangedEmailIntegrationEvent);

        public ProductPropertiesChangedEmailIntegrationEvent(Guid productId, Guid manufacturer, string name,
            decimal price, string description, decimal height, decimal width, decimal length, decimal weigth) : base(
            productId, manufacturer, name, price, description, height, width, length, weigth)
        {
        }
    }
}