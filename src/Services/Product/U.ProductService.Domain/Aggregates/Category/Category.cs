using System;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Aggregates.Category
{

	public class Category : Entity, IAggregateRoot
    {
        public Guid AggregateId => Id;
        public string AggregateTypeName => nameof(Category);   

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool IsDraft { get; private set; }

        private Category()
        {
            Name = string.Empty;
            Description = string.Empty;
            ParentCategoryId = default;
            IsDraft = false;
        }
        
        public Category(Guid id, string name, string description, Guid? parentCategoryId = null) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }
        
        public static Category GetDraftCategory() => new Category
        {
            Id = Guid.Parse("728b6e89-e4b6-40f7-89fc-9a7204dc1300"),
            Name = "DRAFT",
            Description = "Draft category, which purpose is to aggregate newly added products.",
            ParentCategoryId = null,
            IsDraft = true
        };
    }
}
