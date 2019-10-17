using System;

namespace U.ProductService.Application.Categories.Models
{
    public class CategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public Guid? CategoryParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}