using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.EventBus.Events;

namespace U.IntegrationEventLog.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IntegrationEventLogService> _logger;

        private IntegrationEventLogService()
        {
        }

        private IntegrationEventLogContext IntegrationEventLogContext => _serviceProvider.CreateScope()
            .ServiceProvider.GetRequiredService<IntegrationEventLogContext>();
        private readonly List<Type> _eventTypes;

        public IntegrationEventLogService(IServiceProvider serviceProvider,
            ILogger<IntegrationEventLogService> logger) : this()
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _eventTypes = IntegrationEventHelper.GetTypes();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            var integrationEventLogs = await IntegrationEventLogContext.IntegrationEventLogs
                .Where(e => e.State != EventStateEnum.NotPublished &&
                            e.State != EventStateEnum.InProgress)
                .ToListAsync();

            if (!integrationEventLogs.Any())
            {
                return integrationEventLogs;
            }

            foreach (var ieLog in integrationEventLogs)
            {
                var eventType = _eventTypes.Find(t =>
                    string.Equals(t.Name, ieLog.EventTypeShortName, StringComparison.InvariantCultureIgnoreCase));

                ieLog.DeserializeJsonContent(eventType);

                if (ieLog.IntegrationEvent is null)
                {
                    _logger.LogWarning($"Integration Event is null.\n" +
                                       $"Integration Event reflection name = {ieLog?.EventTypeName}\n" +
                                       $"Event Type name found by reflection match: {eventType?.Name}");
                }
            }

            return integrationEventLogs;
        }

        public Task SaveEventAsync<T>(T @event) where T : IntegrationEvent
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event);

            IntegrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = IntegrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            IntegrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }
    }
}