using System;
using MediatR;

namespace U.ProductService.Application.Manufacturers.Commands.DeletePicture
{
    public class DeleteManufacturerPictureCommand : IRequest
    {
        public Guid ManufacturerId { get; private set; }
        public Guid PictureId { get; private set; }

        public DeleteManufacturerPictureCommand(Guid manufacturerId, Guid pictureId)
        {
            ManufacturerId = manufacturerId;
            PictureId = pictureId;
        }
    }
}