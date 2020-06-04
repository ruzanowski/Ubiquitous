using System;

namespace U.ProductService.Domain.Entities.Product
{
    public class ProductPicture
    {
        public Guid ProductPictureId { get; set; }
        public Picture.Picture Picture { get; set; }
        public Guid PictureId { get; set; }
        public Entities.Product.Product Product { get; set; }
        public Guid ProductId { get; set; }
    }
}