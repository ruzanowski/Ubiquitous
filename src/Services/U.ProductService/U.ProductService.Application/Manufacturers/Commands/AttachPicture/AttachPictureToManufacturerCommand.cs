using System;
using MediatR;

namespace U.ProductService.Application.Manufacturers.Commands.AttachPicture
{
    public class AttachPictureToManufacturerCommand : IRequest
    {
        public Guid ManufacturerId { get; set; }

        public Guid PictureId { get; set; }

        public AttachPictureToManufacturerCommand(Guid manufacturerId, Guid pictureId)
        {
            ManufacturerId = manufacturerId;
            PictureId = pictureId;
        }
    }
}