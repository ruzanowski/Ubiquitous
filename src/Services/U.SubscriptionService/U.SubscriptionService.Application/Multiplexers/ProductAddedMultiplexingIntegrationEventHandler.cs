using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events.Product;

namespace U.SubscriptionService.Application.Multiplexers
{
    public class ProductAddedMultiplexingIntegrationEventHandler : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly ILogger<ProductAddedMultiplexingIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;

        public ProductAddedMultiplexingIntegrationEventHandler(ILogger<ProductAddedMultiplexingIntegrationEventHandler> logger, IMapper mapper, IEventBus eventBus)
        {
            _logger = logger;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task Handle(ProductAddedIntegrationEvent @event)
        {
            var signalRIntegrationEvent = _mapper.Map<ProductAddedSignalRIntegrationEvent>(@event);
            _eventBus.Publish(signalRIntegrationEvent);
            _logger.LogInformation($"--- Pushed to SignalR: '{nameof(ProductAddedSignalRIntegrationEvent)} ---");
            await Task.CompletedTask;

//            var emailRIntegrationEvent = _mapper.Map<ProductAddedEmailIntegrationEvent>(@event);
//            _eventBus.Publish(emailRIntegrationEvent);
//            _logger.LogInformation($"--- Pushed to Emails: '{nameof(ProductAddedEmailIntegrationEvent)} ---");
        }
    }
}