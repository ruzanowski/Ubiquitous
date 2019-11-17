using System;
using Newtonsoft.Json;
using U.EventBus.Events;

namespace U.IdentityService.Application.Events
{
    public class SignedIn : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public SignedIn(Guid userId)
        {
            UserId = userId;
        }
    }
}