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
        public Guid PictureId { get; }
        
        public string SeoFileName { get; }

        public ProductPictureAddedDomainEvent(Guid productId, Guid pictureId, string seoFileName)
        {
            ProductId = productId;
            PictureId = pictureId;
            SeoFileName = seoFileName;
        }
    }
}