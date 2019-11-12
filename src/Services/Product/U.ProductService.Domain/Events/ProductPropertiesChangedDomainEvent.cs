using System;
using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Commands.Update;

namespace U.ProductService.Domain.Events
{

    /// <summary>
    /// Event used when product's price changed
    /// </summary>
    public class ProductPropertiesChangedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public Guid Manufacturer { get; set; }
        public List<Variance> Variances { get; set; }

        public ProductPropertiesChangedDomainEvent(Guid productId, Guid manufacturer, List<Variance> variances)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
            Variances = variances;
        }
    }
}