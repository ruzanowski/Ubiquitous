using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Commands.Create.Single;

namespace U.ProductService.Application.Products.Commands.Create.Many
{
    public class CreateManyProductsCommand : IRequest
    {
        public IList<CreateProductCommand> CreateProductCommands { get; set; }
    }
}