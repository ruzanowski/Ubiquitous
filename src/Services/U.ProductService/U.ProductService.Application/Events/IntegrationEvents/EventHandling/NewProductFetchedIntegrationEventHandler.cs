using System.Threading.Tasks;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.ProductService.Application.PendingCommands;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Products.Commands.Update.Single;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Events.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPendingCommands _commands;

        public NewProductFetchedIntegrationEventHandler(IProductRepository productRepository, IPendingCommands commands)
        {
            _productRepository = productRepository;
            _commands = commands;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
            var productId = await _productRepository.GetIdByExternalTupleAsync(
                @event.ExternalSourceName,
                @event.Id);

            if (productId is null)
            {
                var command = new CreateProductCommand(@event.Name,
                    @event.BarCode,
                    @event.Price,
                    @event.Description,
                    new DimensionsDto
                    {
                        Length = @event.Length,
                        Width = @event.Width,
                        Height = @event.Height,
                        Weight = @event.Weight
                    },
                    new ExternalCreation
                    {
                        DuplicationValidated = true,
                        SourceName = @event.ExternalSourceName,
                        SourceId = @event.Id
                    });
                command.SetAsQueueable();
                _commands.Add(command);
            }
            else
            {
                var command = new UpdateProductCommand(
                    productId.Value,
                    @event.Name,
                    @event.Price,
                    @event.Description,
                    new DimensionsDto
                    {
                        Length = @event.Length,
                        Width = @event.Width,
                        Height = @event.Height,
                        Weight = @event.Weight
                    }
                );
                command.SetAsQueueable();
                _commands.Add(command);
            }
        }
    }
}