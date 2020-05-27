using System;
using MediatR;

namespace U.ProductService.Domain.Aggregates.Product.Events
{
    /// <summary>
    /// Event published when Product changed
    /// </summary>
    public class ProductPublishedDomainEvent : INotification
    {
        public Guid ProductId { get; }

        public string Name { get; }

        public decimal Price { get; }

        public Guid Manufacturer { get; }

        public ProductPublishedDomainEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}