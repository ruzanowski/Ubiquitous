using System;
using MediatR;

namespace U.ProductService.Domain.Events
{
    /// <summary>
    /// Event published when Product changed 
    /// </summary>
    public class ProductUnpublishedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        
        public string Name { get; }

        public decimal Price { get; }

        public Guid Manufacturer { get; }

        public ProductUnpublishedDomainEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}