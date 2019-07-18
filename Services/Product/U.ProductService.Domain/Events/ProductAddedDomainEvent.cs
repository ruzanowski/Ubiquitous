using System;
using MediatR;

namespace U.ProductService.Domain.Events
{

    /// <summary>
    /// Event used when new product is added
    /// </summary>
    public class ProductAddedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public Guid Manufacturer { get; }

        public ProductAddedDomainEvent(Guid productId, Guid manufacturer)
        {
            ProductId = productId;
            Manufacturer = manufacturer;
        }
    }
}