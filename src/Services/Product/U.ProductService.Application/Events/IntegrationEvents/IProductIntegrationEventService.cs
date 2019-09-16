using System;
using System.Threading.Tasks;
using U.EventBus.Events;

namespace U.ProductService.Application.Events.IntegrationEvents
{
    public interface IProductIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
