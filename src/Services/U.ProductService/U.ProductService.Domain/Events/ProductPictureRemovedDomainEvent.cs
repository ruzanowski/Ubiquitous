using System;
using MediatR;

namespace U.ProductService.Domain.Events
{
    /// <summary>
    /// Event published whilst picture has been added to Product Aggregate
    /// </summary>
    public class ProductPictureRemovedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public Guid PictureId { get; }

        public ProductPictureRemovedDomainEvent(Guid productId, Guid pictureId)
        {
            ProductId = productId;
            PictureId = pictureId;
        }
    }
}