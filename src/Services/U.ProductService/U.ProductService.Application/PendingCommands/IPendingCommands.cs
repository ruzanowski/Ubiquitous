using U.ProductService.Application.Products.Commands.Create.Many;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Products.Commands.Update.Many;
using U.ProductService.Application.Products.Commands.Update.Single;

namespace U.ProductService.Application.PendingCommands
{
    public interface IPendingCommands
    {
        IPendingCommands Add(CreateProductCommand createProductCommand);
        IPendingCommands Add(UpdateProductCommand updateProductCommand);
        CreateManyProductsCommand GetCreateCommands();
        UpdateManyProductsCommand GetUpdateCommands();
        void Flush();

    }
}