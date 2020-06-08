using MediatR;
using U.ProductService.Application.Manufacturers.Models;

namespace U.ProductService.Application.Manufacturers.Commands.Create
{
    public class CreateManufacturerCommand : IRequest<ManufacturerViewModel>
    {
        public string Name { get;  set; }
        public string Description { get;  set; }

        public CreateManufacturerCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
