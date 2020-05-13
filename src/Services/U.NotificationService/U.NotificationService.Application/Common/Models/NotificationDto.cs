using System;
using System.Linq;
using Newtonsoft.Json;
using U.Common.Jwt.Claims;
using U.Common.Subscription;
using U.EventBus.Events;
using U.EventBus.Events.Notification;
using U.EventBus.Events.Product;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Models
{
    public class NotificationDto
    {
        //needed for messagepack
        public NotificationDto()
        {
        }

        protected NotificationDto(Guid id, IntegrationEvent @event, IntegrationEventType eventType,
            ConfirmationType state, Importancy importancy)
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

        public static class NotifactionFactory
        {
            public static NotificationDto FromNotificationWithPrefferedImportancy(Notification notification,
                Guid currentUserId) =>
                new NotificationDto(notification.Id,
                    DeserializeDependingOnType(notification.IntegrationEvent, notification.IntegrationEventType),
                    notification.IntegrationEventType,
                    notification.Confirmations.FirstOrDefault(x => x.User.Equals(currentUserId))?.ConfirmationType ??
                    ConfirmationType.Unread,
                    notification.Importancies.FirstOrDefault(x => x.UserId.Equals(currentUserId))?.Importancy ??
                    Importancy.Trivial); // many to one conversion

            public static NotificationDto
                GlobalVolatileNotification(Notification notification) =>
                new NotificationDto(notification.Id,
                    notification.IntegrationEventDeserialized,
                    notification.IntegrationEventType,
                    notification.Confirmations.FirstOrDefault()?.ConfirmationType ?? ConfirmationType.Unread,
                    Importancy.Important);

            public static NotificationDto UserDisconnected(UserDto user) =>
                new NotificationDto(user.Id,
                    new UserDisconnectedSignalRIntegrationEvent
                    {
                        Nickname = user.Nickname,
                        Role = user.Role,
                        UserId = user.Id
                    },
                    IntegrationEventType.UserDisconnected,
                    ConfirmationType.Unread,
                    Importancy.Trivial);

            public static NotificationDto UserConnected(UserDto user) =>
                new NotificationDto(user.Id,
                    new UserConnectedSignalRIntegrationEvent
                    {
                        Nickname = user.Nickname,
                        Role = user.Role,
                        UserId = user.Id
                    },
                    IntegrationEventType.UserConnected,
                    ConfirmationType.Unread,
                    Importancy.Trivial);

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
}