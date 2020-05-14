using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
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

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}