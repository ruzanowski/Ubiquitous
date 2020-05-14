using System.Threading.Tasks;
using AutoMapper;
using U.EventBus.Abstractions;
using U.EventBus.Events.Product;

namespace U.SubscriptionService.Application.Multiplexers
{
    public class ProductAddedMultiplexingIntegrationEventHandler : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;

        public ProductAddedMultiplexingIntegrationEventHandler(IMapper mapper, IEventBus eventBus)
        {
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task Handle(ProductAddedIntegrationEvent @event)
        {
            var signalRIntegrationEvent = _mapper.Map<ProductAddedSignalRIntegrationEvent>(@event);
            _eventBus.Publish(signalRIntegrationEvent);
            await Task.CompletedTask;

//            var emailRIntegrationEvent = _mapper.Map<ProductAddedEmailIntegrationEvent>(@event);
//            _eventBus.Publish(emailRIntegrationEvent);
//            _logger.LogInformation($"--- Pushed to Emails: '{nameof(ProductAddedEmailIntegrationEvent)} ---");
        }
    }
}