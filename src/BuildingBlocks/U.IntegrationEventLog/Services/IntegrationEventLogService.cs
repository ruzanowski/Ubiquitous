using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using U.EventBus.Events;

namespace U.IntegrationEventLog.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private IntegrationEventLogService()
        {

        }

        private readonly IntegrationEventLogContext _integrationEventLogContext;
        private readonly List<Type> _eventTypes;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseNpgsql(dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            List<Assembly> allAssemblies =
                Directory.GetFiles(path, "U.*.dll")
                    .Select(dll => AppDomain.CurrentDomain.Load(Assembly.LoadFrom(dll).GetName()))
                    .ToList();

            _eventTypes = allAssemblies.SelectMany(
                x => x.GetTypes()
                    .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                    .ToList()
            ).ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            var integrationEventLogs = await _integrationEventLogContext.IntegrationEventLogs
                .Where(e => e.State == EventStateEnum.NotPublished)
                .OrderBy(o => o.CreationTime)
                .ToListAsync();

            if (!integrationEventLogs.Any())
            {
                return integrationEventLogs;
            }

            return integrationEventLogs.Select(e =>
                    e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)));
        }

        public Task SaveEventAsync<T>(T @event, IDbContextTransaction transaction) where T : IntegrationEvent
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event, transaction?.TransactionId);

            _integrationEventLogContext.Database.UseTransaction(transaction?.GetDbTransaction());
            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
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
            var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }
    }
}