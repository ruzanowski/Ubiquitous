using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class RefreshTokenRevoked : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public RefreshTokenRevoked(Guid userId)
        {
            UserId = userId;
        }
    }
}