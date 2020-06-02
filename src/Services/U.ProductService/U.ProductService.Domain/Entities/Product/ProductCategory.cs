using System;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Entities.Product
{

	public class ProductCategory : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }

        private ProductCategory()
        {
            Name = string.Empty;
            Description = string.Empty;
            ParentCategoryId = default;
        }

        public ProductCategory(Guid id, string name, string description, Guid? parentCategoryId = null) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }

        public static ProductCategory GetDraftCategory() => new ProductCategory
        {
            Id = Guid.Parse("728b6e89-e4b6-40f7-89fc-9a7204dc1300"),
            Name = "DRAFT",
            Description = "Draft productCategory, which purpose is to aggregate newly added products.",
            ParentCategoryId = null
        };
    }
}
