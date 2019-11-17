using System;
using Newtonsoft.Json;
using U.EventBus.Events;

namespace U.IdentityService.Application.Events
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