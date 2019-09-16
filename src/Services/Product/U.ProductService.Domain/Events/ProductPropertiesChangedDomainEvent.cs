using System;
using MediatR;

namespace U.ProductService.Domain.Events
{

    /// <summary>
    /// Event used when product's price changed
    /// </summary>
    public class ProductPropertiesChangedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public string Name { get; }
        
        public decimal Price { get; }

        public ProductPropertiesChangedDomainEvent(Guid productId, string name, decimal price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }
    }
}