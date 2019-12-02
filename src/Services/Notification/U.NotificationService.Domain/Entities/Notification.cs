using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using U.EventBus.Events;
using U.EventBus.Events.Product;

namespace U.NotificationService.Domain.Entities
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
            Confirmations = new List<Confirmation>();
            SetEventType(@event);
        }

        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// For purposes of filtering by IntegrationEventId
        /// </summary>
        public Guid IntegrationEventId { get; private set; }

        public string IntegrationEvent { get; private set; }
        public ICollection<Confirmation> Confirmations { get; private set; }
        public ICollection<UserBasedEventImportancy> Importancies { get; private set; }

        public IntegrationEventType IntegrationEventType { get; private set; }

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
                Guid.Empty); //todo: device Received

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
                Guid.Empty); //todo: device Received

            Confirmations.Add(confirmation);
        }

        public void SetImportancy(Guid userId, Importancy importancy)
        {
            var userBasedEventImportancy = Importancies.FirstOrDefault(x => x.UserId.Equals(userId));

            if (userBasedEventImportancy is null)
            {
                var userImportancy = new UserBasedEventImportancy
                {
                    Importancy = importancy,
                    UserId = userId
                };
                Importancies.Add(userImportancy);
            }
            else
            {
                userBasedEventImportancy.Importancy = importancy;
            }
        }

        public class UserBasedEventImportancy
        {
            public int Id { get; set; }
            public Importancy Importancy { get; set; }
            public Guid UserId { get; set; }
        }
    }
}