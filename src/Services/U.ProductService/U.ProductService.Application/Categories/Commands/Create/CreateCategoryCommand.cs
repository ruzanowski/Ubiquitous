using System;
using MediatR;

namespace U.ProductService.Application.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<Guid>
    {
        public string Name { get;  set; }
        public string Description { get;  set; }
        public Guid? ParentId { get; set; }

        public CreateCategoryCommand()
        {

        }

        public CreateCategoryCommand(string name, string description, Guid? parentId)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
        }
    }
}
