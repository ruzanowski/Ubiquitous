using System;

namespace U.ProductService.Domain.Entities.Manufacturer
{
    public class ManufacturerPicture
    {
        public Guid ManufacturerPictureId { get; set; }
        public Picture.Picture Picture { get; set; }
        public Guid PictureId { get; set; }
        public Entities.Manufacturer.Manufacturer Manufacturer { get; set; }
        public Guid ManufacturerId { get; set; }
    }
}