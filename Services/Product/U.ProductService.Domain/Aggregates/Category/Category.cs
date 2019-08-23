using System;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain
{

	public class Category : Entity, IAggregateRoot
    {
        public Guid AggregateId => Id;
        public string AggregateTypeName => nameof(Category);   

        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }

        private Category()
        {
            Name = string.Empty;
            Description = string.Empty;
            ParentCategoryId = default;
        }
        
        public Category(Guid id, string name, string description, int? parentCategoryId = null) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }


    }
}
