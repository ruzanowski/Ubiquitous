using System;
using System.Collections.Generic;
using U.EventBus.Events;
using U.NotificationService.Application.Builders.Query;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Builders.PendingNotifications
{
    public interface IPendingEventsService
    {
        IPendingEventsService Add(IntegrationEvent notification);
        IList<IntegrationEvent> Get();
        void Flush();

    }
}