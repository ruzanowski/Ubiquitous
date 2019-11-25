using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class AccessTokenRefreshed : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public AccessTokenRefreshed(Guid userId)
        {
            UserId = userId;
        }
    }
}