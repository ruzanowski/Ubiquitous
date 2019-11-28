using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
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