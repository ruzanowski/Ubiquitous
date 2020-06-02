using System;
using MediatR;

namespace U.ProductService.Application.Pictures.Commands.DeletePicture
{
    public class DeletePictureCommand : IRequest
    {
        public Guid PictureId { get; set; }

        public DeletePictureCommand(Guid pictureId)
        {
            PictureId = pictureId;
        }
    }
}