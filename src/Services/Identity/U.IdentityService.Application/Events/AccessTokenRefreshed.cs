using System;
using Newtonsoft.Json;
using U.EventBus.Events;

namespace U.IdentityService.Application.Events
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