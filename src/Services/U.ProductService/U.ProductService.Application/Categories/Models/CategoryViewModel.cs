using System;

namespace U.ProductService.Application.Categories.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}