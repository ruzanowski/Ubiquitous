using System;
using System.Collections.Generic;
using MediatR;
using U.EventBus.Events.Product;

namespace U.ProductService.Domain.Events
{

    /// <summary>
    /// Event used when product's price changed
    /// </summary>
    public class ProductPropertiesChangedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public Guid Manufacturer { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Dimensions Dimensions { get; set; }

        public ProductPropertiesChangedDomainEvent(Guid productId, Guid manufacturer, string name, decimal price, string description, Dimensions dimensions)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
            Name = name;
            Price = price;
            Description = description;
            Dimensions = dimensions;
        }
    }
}