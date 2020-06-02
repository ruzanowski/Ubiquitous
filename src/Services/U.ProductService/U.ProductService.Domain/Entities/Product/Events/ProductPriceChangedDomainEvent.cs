using System;
using MediatR;

namespace U.ProductService.Domain.Entities.Product.Events
{

    /// <summary>
    /// Event used when product's price changed
    /// </summary>
    public class ProductPriceChangedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public decimal PreviousPrice { get; }
        public decimal CurrentPrice { get; }

        public ProductPriceChangedDomainEvent(Guid productId, decimal previousPrice, decimal currentPrice)
        {
            ProductId = productId;
            PreviousPrice = previousPrice;
            CurrentPrice = currentPrice;
        }
    }
}