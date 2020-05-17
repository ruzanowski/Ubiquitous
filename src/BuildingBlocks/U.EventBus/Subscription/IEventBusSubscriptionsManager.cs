using System;
using System.Collections.Generic;
using U.EventBus.Abstractions;
using U.EventBus.Events;

namespace U.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;

        void RemoveSubscription<T, TH>()
             where TH : IIntegrationEventHandler<T>
             where T : IntegrationEvent;

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();
    }
}