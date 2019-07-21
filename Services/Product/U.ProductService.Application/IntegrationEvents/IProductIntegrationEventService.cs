using System;
using System.Threading.Tasks;
using U.EventBus.Events;

namespace U.ProductService.Application.IntegrationEvents
{
    public interface IProductIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
