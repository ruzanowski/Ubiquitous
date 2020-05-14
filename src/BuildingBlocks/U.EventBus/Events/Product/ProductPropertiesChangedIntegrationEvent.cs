using System;
using System.Collections.Generic;
using U.Common.Subscription;

namespace U.EventBus.Events.Product
{
    public class ProductPropertiesChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }

        public Guid Manufacturer { get; }

        public IList<Variance> Variances { get; set; }

        public ProductPropertiesChangedIntegrationEvent(Guid productId, Guid manufacturer, IList<Variance> variances)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
            Variances = variances;
        }
        public override string MethodTag => nameof(ProductPropertiesChangedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.ProductPropertiesChangedIntegrationEvent;
    }

    public sealed class ProductPropertiesChangedSignalRIntegrationEvent : ProductPropertiesChangedIntegrationEvent
    {
        public ProductPropertiesChangedSignalRIntegrationEvent(Guid productId, Guid manufacturer, List<Variance> variances) : base(productId, manufacturer, variances)
        {
        }
        public override string MethodTag => nameof(ProductPropertiesChangedSignalRIntegrationEvent);

    }

    public sealed class ProductPropertiesChangedEmailIntegrationEvent : ProductPropertiesChangedIntegrationEvent
    {
        public ProductPropertiesChangedEmailIntegrationEvent(Guid productId, Guid manufacturer, List<Variance> variances) : base(productId, manufacturer, variances)
        {
        }
        public override string MethodTag => nameof(ProductPropertiesChangedEmailIntegrationEvent);

    }
}