using System.Threading.Tasks;
using U.EventBus.Events;

namespace U.EventBus.Abstractions
{
    public interface ICarrierIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent: IntegrationEvent
    {
        Task Handle(Carrier<TIntegrationEvent> carrier);
    }

    public interface IIntegrationEventHandler
    {
    }
}
