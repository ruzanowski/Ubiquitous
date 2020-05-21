using System.Collections.Generic;
using System.Linq;
using U.EventBus.Events;
using U.NotificationService.Application.Builders.PendingNotifications;

namespace U.NotificationService.Application.Services.PendingEvents
{
    public class PendingEventsService : IPendingEventsService
    {
        private readonly IList<IntegrationEvent> _pendingEvents = new List<IntegrationEvent>();

        public IPendingEventsService Add(IntegrationEvent notification)
        {
            _pendingEvents.Add(notification);
            return this;
        }

        public IList<IntegrationEvent> Get()
        {
            return _pendingEvents.ToList();
        }

        public void Flush()
        {
            _pendingEvents.Clear();
        }
    }
}