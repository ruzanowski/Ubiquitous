using System;

namespace U.ProductService.Domain.Entities.Picture
{
    public class ProductPicture
    {
        public Guid ProductPictureId { get; set; }
        public Picture Picture { get; set; }
        public Guid PictureId { get; set; }
        public Product.Product Product { get; set; }
        public Guid ProductId { get; set; }
    }
}