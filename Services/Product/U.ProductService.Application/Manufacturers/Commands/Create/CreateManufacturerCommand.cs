using System;
using MediatR;
using Newtonsoft.Json;

namespace U.ProductService.Application.Manufacturers.Commands.Create
{
    public class CreateManufacturerCommand : IRequest<Guid>
    {
        public string Name { get;  set; }
        public string Description { get;  set; }

        [JsonConstructor]
        public CreateManufacturerCommand()
        {
                
        }

        public CreateManufacturerCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
