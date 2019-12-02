using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events.Product;

namespace U.SubscriptionService.Application.Multiplexers
{
    public class ProductPublishedMultiplexingIntegrationEventHandler : IIntegrationEventHandler<ProductPublishedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedMultiplexingIntegrationEventHandler> _logger;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public ProductPublishedMultiplexingIntegrationEventHandler(ILogger<ProductPublishedMultiplexingIntegrationEventHandler> logger,
            IEventBus eventBus,
             IMapper mapper)
        {
            _logger = logger;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task Handle(ProductPublishedIntegrationEvent @event)
        {
            var signalRIntegrationEvent = _mapper.Map<ProductPublishedSignalRIntegrationEvent>(@event);
            _eventBus.Publish(signalRIntegrationEvent);
            _logger.LogInformation($"--- Pushed to SignalR: '{nameof(ProductPublishedSignalRIntegrationEvent)} ---");

//            var emailRIntegrationEvent = _mapper.Map<ProductPublishedEmailIntegrationEvent>(@event);
//            _eventBus.Publish(emailRIntegrationEvent);
//            _logger.LogInformation($"--- Pushed to Emails: '{nameof(ProductPublishedEmailIntegrationEvent)} ---");
        }
    }
}