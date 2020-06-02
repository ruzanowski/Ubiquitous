using System.Collections.Generic;
using System.Linq;
using U.EventBus.Events;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Update;

namespace U.ProductService.Application.Services
{
    public class PendingCommands : IPendingCommands
    {
        private readonly IList<CreateProductCommand> _pendingCreateCommands = new List<CreateProductCommand>();
        private readonly IList<UpdateProductCommand> _pendingUpdateCommands = new List<UpdateProductCommand>();


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

        public IList<CreateProductCommand> GetCreateCommands()
        {
            return _pendingCreateCommands.ToList();
        }
        public IList<UpdateProductCommand> GetUpdateCommands()
        {
            return _pendingUpdateCommands.ToList();
        }

        public void Flush()
        {
            _pendingCreateCommands.Clear();
            _pendingUpdateCommands.Clear();
        }
    }
}