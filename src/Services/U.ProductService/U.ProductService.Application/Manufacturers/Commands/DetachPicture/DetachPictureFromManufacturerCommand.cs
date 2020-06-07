using System;
using MediatR;

namespace U.ProductService.Application.Manufacturers.Commands.DetachPicture
{
    public class DetachPictureFromManufacturerCommand : IRequest
    {
        public Guid ManufacturerId { get; private set; }
        public Guid PictureId { get; private set; }

        public DetachPictureFromManufacturerCommand(Guid manufacturerId, Guid pictureId)
        {
            ManufacturerId = manufacturerId;
            PictureId = pictureId;
        }
    }
}