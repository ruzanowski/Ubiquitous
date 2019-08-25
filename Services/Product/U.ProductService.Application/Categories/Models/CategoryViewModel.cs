using System;

namespace U.ProductService.Application.Categories.Models
{
    public class CategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}