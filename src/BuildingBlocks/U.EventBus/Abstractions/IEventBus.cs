using U.EventBus.Events;

namespace U.EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish<T>(T carrier) where T: IntegrationEvent;
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
