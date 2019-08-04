using System;
using MediatR;

namespace U.ProductService.Domain.Events
{

    /// <summary>
    /// Event published whilst new product is added to Products' Service Database
    /// </summary>
    public class ProductAddedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        
        public string Name { get; }
        
        public decimal Price { get; }
        
        public Guid Manufacturer { get; }

        public ProductAddedDomainEvent(Guid productId, string name, decimal price, Guid manufacturer)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }
    }
}