using System;
using MediatR;

namespace U.ProductService.Domain.Entities.Product.Events
{
    /// <summary>
    /// Event published whilst new product is added
    /// </summary>
    public class ProductAddedDomainEvent : INotification
    {
        public Guid ProductId { get; }

        public string Name { get; }
        public decimal Price { get; }
        public Guid Manufacturer { get; }
        public Guid CategoryId { get; }
        public string ExternalSourceName { get; }
        public string ExternalId { get; }

        private ProductAddedDomainEvent()
        {
        }

        public ProductAddedDomainEvent(Guid productId,
            string name,
            decimal price,
            Guid manufacturer,
            Guid categoryId,
            string externalSourceName, string externalId)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
            CategoryId = categoryId;
            ExternalSourceName = externalSourceName;
            ExternalId = externalId;
        }
    }
}