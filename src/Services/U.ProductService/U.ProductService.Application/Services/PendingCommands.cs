using System.Collections.Generic;
using System.Linq;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Create.Many;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Commands.Update.Many;
using U.ProductService.Application.Products.Commands.Update.Single;

namespace U.ProductService.Application.Services
{
    public class PendingCommands : IPendingCommands
    {
        private readonly ICollection<CreateProductCommand> _pendingCreateCommands = new HashSet<CreateProductCommand>();
        private readonly ICollection<UpdateProductCommand> _pendingUpdateCommands = new HashSet<UpdateProductCommand>();


        public IPendingCommands Add(CreateProductCommand createProductCommand)
        {
            _pendingCreateCommands.Add(createProductCommand);
            return this;
        }
        public IPendingCommands Add(UpdateProductCommand command)
        {
            _pendingUpdateCommands.Add(command);
            return this;
        }

        public CreateManyProductsCommand GetCreateCommands()
        {
            return new CreateManyProductsCommand
            {
                CreateProductCommands = _pendingCreateCommands.ToList()
            };
        }
        public UpdateManyProductsCommand GetUpdateCommands()
        {
            return new UpdateManyProductsCommand
            {
                UpdateProductCommands = _pendingUpdateCommands.ToList()
            };
        }

        public void Flush()
        {
            _pendingCreateCommands.Clear();
            _pendingUpdateCommands.Clear();
        }
    }
}