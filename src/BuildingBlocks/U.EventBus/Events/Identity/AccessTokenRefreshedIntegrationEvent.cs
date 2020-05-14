using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using U.Common.Subscription;

namespace U.EventBus.Events.Identity
{
    public class AccessTokenRefreshedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public AccessTokenRefreshedIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }
        [NotMapped]
        public override string MethodTag => nameof(AccessTokenRefreshedIntegrationEvent);

        public override IntegrationEventType EventType => IntegrationEventType.AccessTokenRefreshedIntegrationEvent;
    }
}