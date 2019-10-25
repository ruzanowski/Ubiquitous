using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using U.EventBus.Abstractions;
using U.EventBus.Events;
using U.IntegrationEventLog.Services;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Events.IntegrationEvents
{
    public class ProductIntegrationEventService : IProductIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly ProductContext _productContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<ProductIntegrationEventService> _logger;

        public ProductIntegrationEventService(IEventBus eventBus,
            ProductContext productContext,
            ILogger<ProductIntegrationEventService> logger)
        {
            _productContext = productContext ?? throw new ArgumentNullException(nameof(productContext));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventLogService = new IntegrationEventLogService(productContext.Database.GetDbConnection());
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync();

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogDebug("----- Publishing integration event: {IntegrationEventId}", logEvt.EventId);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from ProductService", logEvt.EventId);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            await _eventLogService.SaveEventAsync(evt, _productContext.GetCurrentTransaction());
        }
    }
}
