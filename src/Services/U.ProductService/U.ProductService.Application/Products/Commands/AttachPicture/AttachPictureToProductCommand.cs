using System;
using MediatR;

namespace U.ProductService.Application.Products.Commands.AttachPicture
{
    //marker
    public class AttachPictureToProductCommand : IRequest
    {
        public AttachPictureToProductCommand(Guid id, Guid pictureId)
        {
            Id = id;
            PictureId = pictureId;
        }

        public Guid Id { get; private set; }
        public Guid PictureId { get; private set; }
    }
}
