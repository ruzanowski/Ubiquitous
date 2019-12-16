using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.EventBus.Events.Fetch;

namespace U.FetchService.Commands.ForwardProducts
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ForwardDataCommandHandler : IRequestHandler<ForwardDataCommand>
    {
        private readonly IEventBus _bus;
        private readonly ILogger<ForwardDataCommandHandler> _logger;

        public ForwardDataCommandHandler(IEventBus bus, ILogger<ForwardDataCommandHandler> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task<Unit> Handle(ForwardDataCommand command, CancellationToken cancellationToken)
        {
            foreach (var product in command.Data)
            {
                var @event = new NewProductFetchedIntegrationEvent(product.Name,
                    product.ManufacturerId,
                    product.ProductUniqueCode,
                    product.InStock,
                    product.PriceInTax,
                    product.Description,
                    product.Length,
                    product.Width,
                    product.Height,
                    product.Weight,
                    product.MainPictureId,
                    product.CategoryId,
                    product.Id);

                _logger.LogInformation(
                    "----- Publishing integration event: {IntegrationEventId} from 'FetchService' - ({@IntegrationEvent})",
                    @event.Id, @event);

                //fire and forget
                _bus.Publish(@event);
            }

            await Task.CompletedTask;

            return Unit.Value;
        }
    }
}