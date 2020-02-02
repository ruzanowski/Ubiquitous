using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events.Product;

namespace U.SubscriptionService.Application.Multiplexers
{
    public class ProductPropertiesChangedMultiplexingIntegrationEventHandler : IIntegrationEventHandler<ProductPropertiesChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPublishedMultiplexingIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;

        public ProductPropertiesChangedMultiplexingIntegrationEventHandler(ILogger<ProductPublishedMultiplexingIntegrationEventHandler> logger, IMapper mapper, IEventBus eventBus)
        {
            _logger = logger;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task Handle(ProductPropertiesChangedIntegrationEvent @event)
        {
            var signalRIntegrationEvent = _mapper.Map<ProductPropertiesChangedSignalRIntegrationEvent>(@event);
            _eventBus.Publish(signalRIntegrationEvent);
            _logger.LogInformation($"--- Pushed to SignalR: '{nameof(ProductPropertiesChangedSignalRIntegrationEvent)} ---");
            await Task.CompletedTask;

//            var emailRIntegrationEvent = _mapper.Map<ProductPropertiesChangedEmailIntegrationEvent>(@event);
//            _eventBus.Publish(emailRIntegrationEvent);
//            _logger.LogInformation($"--- Pushed to Emails: '{nameof(ProductPropertiesChangedEmailIntegrationEvent)} ---");
        }
    }
}