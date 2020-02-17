using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands.DeletePicture
{
    public class DeleteProductPictureCommand : IRequest
    {
        public Guid ProductId { get; private set; }
        public Guid PictureId { get; private set; }

        public DeleteProductPictureCommand(Guid productId, Guid pictureId)
        {
            ProductId = productId;
            PictureId = pictureId;
        }
    }
}