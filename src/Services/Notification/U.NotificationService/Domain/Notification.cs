using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using U.EventBus.Events;
using U.NotificationService.Application.Hub;
using U.NotificationService.Application.Models;
using U.NotificationService.IntegrationEvents.ProductAdded;
using U.NotificationService.IntegrationEvents.ProductPropertiesChanged;
using U.NotificationService.IntegrationEvents.ProductPublished;

namespace U.NotificationService.Domain
{
    public class Notification
    {
        //necessary for ef migrations
        private Notification()
        {

        }

        public Notification(IntegrationEvent @event)
        {
            CreationDate = DateTime.UtcNow;
            IntegrationEventId = @event.Id;
            IntegrationEvent = JsonConvert.SerializeObject(@event);
            SetEventType(@event);
            Importancy = Importancy.Trivial; // todo: ImportancyService or Settings per user.
        }

        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// For purposes of filtering by IntegrationEventId
        /// </summary>
        public Guid IntegrationEventId { get; set; }
        public string IntegrationEvent { get; set; }
        public ICollection<Confirmation> Confirmations { get; set; }
        public IntegrationEventType IntegrationEventType { get; set; }
        public Importancy Importancy { get; private set; }


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

        public void ChangeStateToRead(Guid userId)
        {
            var isReadAlready = Confirmations.Any(x =>
                x.User.Equals(userId) && x.ConfirmationType == ConfirmationType.Read);

            if (isReadAlready)
            {
                return;
            }

            var confirmation = new Confirmation(userId,
                Id,
                ConfirmationType.Read,
                Guid.NewGuid()); //todo: device Received

            Confirmations.Add(confirmation);
        }

        public void ChangeStateToHidden(Guid userId)
        {
            var isReadAlready = Confirmations.Any(x =>
                x.User.Equals(userId) && x.ConfirmationType == ConfirmationType.Read);

            if (isReadAlready)
            {
                return;
            }

            var confirmation = new Confirmation(userId,
                Id,
                ConfirmationType.Hidden,
                Guid.NewGuid()); //todo: device Received

            Confirmations.Add(confirmation);
        }

        public void SetImportancy(Importancy importancy)
        {
            Importancy = importancy;
        }
    }
}