using System;
using System.Collections.Generic;
using U.EventBus.Events;
using U.ProductService.Domain.Helpers;

namespace U.ProductService.Application.Events.IntegrationEvents.Events
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
}