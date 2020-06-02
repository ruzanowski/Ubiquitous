using U.EventBus.Events;

namespace U.EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T: IntegrationEvent;
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }

    public class NoEventBus : IEventBus
    {
        public void Publish<T>(T @event) where T : IntegrationEvent
        {
        }

        public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
        }

        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
        }
    }
}
