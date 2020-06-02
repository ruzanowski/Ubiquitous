using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Services;
using U.ProductService.Domain;

namespace U.ProductService.Application.Events.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly IPendingCommands _commands;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator,
            IProductRepository productRepository, IPendingCommands commands)
        {
            _mediator = mediator;
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