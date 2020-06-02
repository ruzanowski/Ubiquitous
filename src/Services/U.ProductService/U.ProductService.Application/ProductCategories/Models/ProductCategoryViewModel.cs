using System;

namespace U.ProductService.Application.ProductCategories.Models
{
    public class ProductCategoryViewModel
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}