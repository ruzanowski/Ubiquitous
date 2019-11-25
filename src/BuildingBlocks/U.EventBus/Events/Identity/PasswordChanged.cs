using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class PasswordChanged : IntegrationEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public PasswordChanged(Guid userId)
        {
            UserId = userId;
        }
    }
}