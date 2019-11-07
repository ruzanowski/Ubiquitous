using System;
using U.EventBus.Events;
using U.NotificationService.IntegrationEvents.ProductAdded;
using U.NotificationService.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Application.Hub
{
    public class NotificationDto
    {
        public NotificationDto(Guid id, IntegrationEvent @event, IntegrationEventType eventType)
        {
            Id = id;
            Event = @event;
            EventType = eventType;
            CreationTime = DateTime.UtcNow;
        }

        public IntegrationEvent Event { get; set; }
        public Guid Id { get; set; }
        public IntegrationEventType EventType { get; set; }
        public DateTime CreationTime { get; set; }
    }
}