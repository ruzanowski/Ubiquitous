using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Jaeger.Thrift.Crossdock;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using U.Common.Subscription;
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
            IntegrationEventDeserialized = @event;
            Confirmations = new List<Confirmation>();
            IntegrationEventType = @event.EventType;
            MethodTag = @event.MethodTag;
        }

        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// For purposes of filtering by IntegrationEventId
        /// </summary>
        public Guid IntegrationEventId { get; private set; }

        public string IntegrationEvent { get; private set; }
        [NotMapped]
        public IntegrationEvent IntegrationEventDeserialized { get; private set; }
        [NotMapped]
        public string MethodTag { get; private set; }

        public ICollection<Confirmation> Confirmations { get; private set; }
        public ICollection<UserBasedEventImportancy> Importancies { get; private set; }

        public IntegrationEventType IntegrationEventType { get; private set; }
        public int TimesSent { get; private set; }

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

        public void IncrementProcessedTimes(int recipients = 1)
        {
            TimesSent += recipients;
        }
    }

}