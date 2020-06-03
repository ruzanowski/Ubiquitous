using System;

namespace U.ProductService.Domain.Entities.Picture
{
    public class ManufacturerPicture
    {
        public Guid ManufacturerPictureId { get; set; }
        public Picture Picture { get; set; }
        public Guid PictureId { get; set; }
        public Manufacturer.Manufacturer Manufacturer { get; set; }
        public Guid ManufacturerId { get; set; }
    }
}