﻿using System;
using MediatR;

namespace U.ProductService.Domain.Events
{

    /// <summary>
    /// Event used when product's price changed
    /// </summary>
    public class ProductPriceChangedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public decimal CurrentPrice { get; }

        public ProductPriceChangedDomainEvent(Guid productId, decimal currentPrice)
        {
            ProductId = productId;
            CurrentPrice = currentPrice;
        }
    }
}