using System;
using Newtonsoft.Json;

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
    }
}