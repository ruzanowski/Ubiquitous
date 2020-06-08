using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands.DetachPicture
{
    public class DetachPictureToProductCommand : IRequest
    {
        public DetachPictureToProductCommand(Guid id, Guid pictureId)
        {
            Id = id;
            PictureId = pictureId;
        }

        public Guid Id { get; private set; }
        public Guid PictureId { get; private set; }
    }
}
