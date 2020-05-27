using System.Collections.Generic;
using U.EventBus.Events;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Update;

namespace U.ProductService.Application.Services
{
    public interface IPendingCommands
    {
        IPendingCommands Add(CreateProductCommand createProductCommand);
        IPendingCommands Add(UpdateProductCommand updateProductCommand);
        IList<CreateProductCommand> GetCreateCommands();
        IList<UpdateProductCommand> GetUpdateCommands();
        void Flush();

    }
}