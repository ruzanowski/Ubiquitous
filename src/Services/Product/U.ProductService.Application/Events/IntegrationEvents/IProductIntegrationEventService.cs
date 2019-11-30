using System.Threading.Tasks;
using U.EventBus.Events;

namespace U.ProductService.Application.Events.IntegrationEvents
{
    public interface IProductIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync<T>(Carrier<T> evt) where T : IntegrationEvent;
    }
}
