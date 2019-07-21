using System;
using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.FetchService.Api.IntegrationEvents;
using U.ProductService.Application.Commands;

namespace U.ProductService.Application.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEventHandler : IIntegrationEventHandler<NewProductFetchedIntegrationEvent>
    {
        private IMediator _mediator;

        public NewProductFetchedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(NewProductFetchedIntegrationEvent @event)
        {
            
            var createProduct = new CreateProductCommand(DateTime.Now, null, null, null, null);

            await _mediator.Send(createProduct);
        }
    }
}