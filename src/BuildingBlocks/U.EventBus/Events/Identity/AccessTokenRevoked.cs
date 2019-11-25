using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class AccessTokenRevoked : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public AccessTokenRevoked(Guid userId)
        {
            UserId = userId;
        }
    }
}