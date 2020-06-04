using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Commands.Update.Single;

namespace U.ProductService.Application.Products.Commands.Update.Many
{
    public class UpdateManyProductsCommand : IRequest
    {
        public IList<UpdateProductCommand> UpdateProductCommands { get; set; }
    }
}