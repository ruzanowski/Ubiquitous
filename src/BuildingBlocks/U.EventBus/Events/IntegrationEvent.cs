using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using U.Common.Subscription;

namespace U.EventBus.Events
{
    public abstract class IntegrationEvent
    {
        [NotMapped]
        public abstract string MethodTag { get; }
        public abstract IntegrationEventType EventType { get; }
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonIgnore]
        public Guid Id { get; protected set; }

        public DateTime CreationDate { get; private set; }
    }
}