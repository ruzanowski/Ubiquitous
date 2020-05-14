using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Events.IntegrationEvents;

namespace U.ProductService.Application.Infrastructure.Behaviours
{
    public class EventPublishBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IProductIntegrationEventService _productIntegrationEventService;

        public EventPublishBehaviour(IProductIntegrationEventService productIntegrationEventService)
        {
            _productIntegrationEventService = productIntegrationEventService ??
                                              throw new ArgumentException(nameof(productIntegrationEventService));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            //await _productIntegrationEventService.PublishEventsThroughEventBusAsync();

            return response;
        }
    }
}