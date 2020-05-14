using System;
using Newtonsoft.Json;
using U.Common.Subscription;

namespace U.EventBus.Events.Identity
{
    public class SignedInIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public SignedInIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }

        public override string MethodTag => nameof(SignedInIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.SignedIn;
    }
}