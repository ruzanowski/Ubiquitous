using U.EventBus.Events;

namespace U.EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish<T>(Carrier<T> carrier) where T: IntegrationEvent;
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : ICarrierIntegrationEventHandler<T>;
        void Unsubscribe<T, TH>()
            where TH : ICarrierIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
