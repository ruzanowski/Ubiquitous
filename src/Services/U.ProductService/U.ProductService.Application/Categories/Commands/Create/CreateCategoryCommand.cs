using System;
using MediatR;
using U.ProductService.Application.Categories.Models;

namespace U.ProductService.Application.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<CategoryViewModel>
    {
        public string Name { get; private set; }
        public string Description { get;  private set; }
        public Guid? ParentId { get; private set; }


        public CreateCategoryCommand(string name, string description, Guid? parentId = null)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
        }
    }
}
