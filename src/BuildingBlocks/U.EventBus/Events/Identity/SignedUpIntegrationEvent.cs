using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class SignedUpIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Role { get; }

        [JsonConstructor]
        public SignedUpIntegrationEvent(Guid userId, string email, string role)
        {
            UserId = userId;
            Email = email;
            Role = role;
        }
    }
}