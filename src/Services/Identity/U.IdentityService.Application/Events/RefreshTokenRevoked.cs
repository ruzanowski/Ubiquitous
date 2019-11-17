using System;
using Newtonsoft.Json;
using U.EventBus.Events;

namespace U.IdentityService.Application.Events
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