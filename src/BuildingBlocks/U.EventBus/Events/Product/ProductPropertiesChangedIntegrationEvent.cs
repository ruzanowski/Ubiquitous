using System;
using System.Collections.Generic;

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
    }

    public sealed class ProductPropertiesChangedSignalRIntegrationEvent : ProductPropertiesChangedIntegrationEvent
    {
        public ProductPropertiesChangedSignalRIntegrationEvent(Guid productId, Guid manufacturer, List<Variance> variances) : base(productId, manufacturer, variances)
        {
        }
    }

    public sealed class ProductPropertiesChangedEmailIntegrationEvent : ProductPropertiesChangedIntegrationEvent
    {
        public ProductPropertiesChangedEmailIntegrationEvent(Guid productId, Guid manufacturer, List<Variance> variances) : base(productId, manufacturer, variances)
        {
        }
    }
}