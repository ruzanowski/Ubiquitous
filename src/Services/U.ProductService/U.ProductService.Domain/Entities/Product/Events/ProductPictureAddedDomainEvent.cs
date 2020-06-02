using System;
using MediatR;

namespace U.ProductService.Domain.Entities.Product.Events
{
    /// <summary>
    /// Event published whilst picture has been added to Product Aggregate
    /// </summary>
    public class ProductPictureAddedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public Guid PictureId { get; }

        public ProductPictureAddedDomainEvent(Guid productId, Guid pictureId)
        {
            ProductId = productId;
            PictureId = pictureId;
        }
    }
}