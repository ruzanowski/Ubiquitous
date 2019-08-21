using System;
using System.Collections.Generic;
using U.ProductService.Domain;

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
        public DateTime CreatedDateTime { get;  set; }
        public DateTime? LastFullUpdateDateTime { get;  set; }
        public Dimensions Dimensions { get;  set; }
        public Guid ManufacturerId { get;  set; }
        public IReadOnlyCollection<Picture> Pictures { get;  set; }
    }
}