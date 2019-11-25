using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class SignedUp : IntegrationEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Role { get; }

        [JsonConstructor]
        public SignedUp(Guid userId, string email, string role)
        {
            UserId = userId;
            Email = email;
            Role = role;
        }
    }
}