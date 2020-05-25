using System.Threading.Tasks;
using U.EventBus.Events;

namespace U.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent: IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
