using System;
using Newtonsoft.Json;

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
    }
}