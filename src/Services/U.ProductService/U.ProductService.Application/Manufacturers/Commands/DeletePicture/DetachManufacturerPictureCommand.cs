using System;
using MediatR;

namespace U.ProductService.Application.Manufacturers.Commands.DeletePicture
{
    public class DetachManufacturerPictureCommand : IRequest
    {
        public Guid ManufacturerId { get; private set; }
        public Guid PictureId { get; private set; }

        public DetachManufacturerPictureCommand(Guid manufacturerId, Guid pictureId)
        {
            ManufacturerId = manufacturerId;
            PictureId = pictureId;
        }
    }
}