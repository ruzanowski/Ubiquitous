using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using U.EventBus.Events;

namespace U.IntegrationEventLog.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly DbConnection _dbConnection;
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        private readonly List<Type> _eventTypes;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseNpgsql(_dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            List<Assembly> allAssemblies = Directory.GetFiles(path, "U.*.dll").Select(dll => Assembly.LoadFile(dll)).ToList();

            _eventTypes = allAssemblies.SelectMany(
                x => x.GetTypes()
                    .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                    .ToList()
            ).ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            return await _integrationEventLogContext.IntegrationEventLogs
                .Where(e => e.State == EventStateEnum.NotPublished)
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t=> t.Name == e.EventTypeShortName)))
                .ToListAsync();              
        }

        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
        {
//            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

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

            if(status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }
    }
}
