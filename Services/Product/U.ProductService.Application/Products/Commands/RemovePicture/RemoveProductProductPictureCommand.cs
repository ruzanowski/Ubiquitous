using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands.RemovePicture
{
    public class RemoveProductPictureCommand : IRequest<bool>
    {
        public Guid ProductId { get; private set; }

        public RemoveProductPictureCommand(Guid productId)
        {
            ProductId = productId;
        }
    }
}