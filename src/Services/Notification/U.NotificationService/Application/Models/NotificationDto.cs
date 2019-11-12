using System;
using System.Linq;
using Newtonsoft.Json;
using U.EventBus.Events;
using U.NotificationService.Application.Hub;
using U.NotificationService.Domain;
using U.NotificationService.IntegrationEvents.ProductAdded;
using U.NotificationService.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Application.Models
{
    public class NotificationDto
    {
        public NotificationDto(Guid id, IntegrationEvent @event, IntegrationEventType eventType, ConfirmationType state, Importancy importancy)
        {
            Id = id;
            Event = @event;
            EventType = eventType;
            CreationTime = DateTime.UtcNow;
            State = state;
            Importancy = importancy;
        }

        public IntegrationEvent Event { get; private set; }
        public Guid Id { get; private set; }
        public IntegrationEventType EventType { get; private set; }
        public DateTime CreationTime { get; private set; }
        public ConfirmationType State { get; private set; }
        public Importancy Importancy { get; private set; }
    }

    public static class NotifactionFactory
    {
        public static NotificationDto FromStoredNotification(Notification notification) =>
            new NotificationDto(notification.Id,
                DeserializeDependingOnType(notification.IntegrationEvent, notification.IntegrationEventType),
                notification.IntegrationEventType,
                notification.Confirmations.FirstOrDefault()?.ConfirmationType ?? ConfirmationType.Unread,
                notification.Importancy);

        static IntegrationEvent DeserializeDependingOnType(string @event, IntegrationEventType eventType)
        {
            switch (eventType)
            {
                case IntegrationEventType.ProductPublishedIntegrationEvent:
                    return JsonConvert.DeserializeObject<ProductPublishedIntegrationEvent>(@event);
                case IntegrationEventType.ProductPropertiesChangedIntegrationEvent:
                    return JsonConvert.DeserializeObject<ProductPropertiesChangedIntegrationEvent>(@event);
                case IntegrationEventType.ProductAddedIntegrationEvent:
                    return JsonConvert.DeserializeObject<ProductAddedIntegrationEvent>(@event);
                // ReSharper disable once RedundantCaseLabel
                default:
                case IntegrationEventType.Unknown:
                    return JsonConvert.DeserializeObject<IntegrationEvent>(@event);
            }
        }
    }
}