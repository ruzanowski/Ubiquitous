using System;
using System.Collections.Generic;

namespace U.EventBus.Events.Product
{
    public class ProductPropertiesChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }

        public Guid Manufacturer { get; }

        public List<Variance> Variances { get; set; }


        public ProductPropertiesChangedIntegrationEvent(Guid productId, Guid manufacturer, List<Variance> variances)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
            Variances = variances;
        }
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object ValueA { get; set; }
        public object ValueB { get; set; }
    }


}