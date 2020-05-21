using System.Collections.Generic;
using U.EventBus.Events;

namespace U.NotificationService.Application.Builders.PendingNotifications
{
    public interface IPendingEventsService
    {
        IPendingEventsService Add(IntegrationEvent notification);
        IList<IntegrationEvent> Get();
        void Flush();

    }
}