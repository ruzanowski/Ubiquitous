using System;
using System.Linq;
using Newtonsoft.Json;
using U.EventBus.Events;
using U.EventBus.Events.Notification;
using U.EventBus.Events.Product;
using U.NotificationService.Application.Services.Users;
using U.NotificationService.Domain.Entities;

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

        public static NotificationDto UserDisconnected(UserDto user) =>
            new NotificationDto(user.Id,
                new UserDisconnected
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
                new UserConnectedIntegrationEvent
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