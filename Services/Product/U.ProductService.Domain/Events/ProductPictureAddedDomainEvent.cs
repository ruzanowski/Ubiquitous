using System;
using MediatR;

namespace U.ProductService.Domain.Events
{
    /// <summary>
    /// Event published whilst picture has been added to Product Aggregate
    /// </summary>
    public class ProductPictureAddedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        
        public string SeoFileName { get; }

        public ProductPictureAddedDomainEvent(Guid productId, string seoFileName)
        {
            ProductId = productId;
            SeoFileName = seoFileName;
        }
    }
}