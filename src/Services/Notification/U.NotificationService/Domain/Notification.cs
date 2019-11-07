using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using U.EventBus.Events;
using U.NotificationService.Application.Hub;
using U.NotificationService.IntegrationEvents.ProductAdded;
using U.NotificationService.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Domain
{
    public class Notification
    {
        //needed for migrations
        private Notification()
        {

        }

        public Notification(IntegrationEvent @event)
        {
            CreationDate = DateTime.UtcNow;
            IntegrationEventId = @event.Id;
            IntegrationEvent = JsonConvert.SerializeObject(@event);
            SetEventType(@event);
        }

        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// For purposes of filtering by IntegrationEventId
        /// </summary>
        public Guid IntegrationEventId { get; set; }
        public string IntegrationEvent { get; set; }
        public IEnumerable<Confirmation> Confirmations { get; set; }
        public IntegrationEventType IntegrationEventType { get; set; }

        private void SetEventType(IntegrationEvent @event)
        {
            switch (@event)
            {
                case ProductAddedIntegrationEvent _:
                    IntegrationEventType = IntegrationEventType.ProductAddedIntegrationEvent;
                    break;
                case ProductPublishedIntegrationEvent _:
                    IntegrationEventType = IntegrationEventType.ProductPublishedIntegrationEvent;
                    break;
                case ProductPropertiesChangedIntegrationEvent _:
                    IntegrationEventType = IntegrationEventType.ProductPropertiesChangedIntegrationEvent;
                    break;
                default:
                    IntegrationEventType = IntegrationEventType.Unknown;
                    break;
            }
        }
    }
}