using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using U.EventBus.Events;

namespace U.IntegrationEventLog.Services
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync();
        Task SaveEventAsync<T>(T @event, IDbContextTransaction transaction) where T : IntegrationEvent;
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsInProgressAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
    }
}
