using System;
using System.Collections.Generic;
using U.ProductService.Application.Pictures;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.ProductCategories.Models;

namespace U.ProductService.Application.Products.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get;  set; }
        public string BarCode { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public bool IsPublished { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? LastUpdatedAt { get;  set; }
        public DimensionsDto Dimensions { get;  set; }
        public Guid ManufacturerId { get;  set; }
        public ProductCategoryViewModel ProductCategory { get;  set; }
        public ICollection<PictureViewModel> Pictures { get;  set; }
    }
}