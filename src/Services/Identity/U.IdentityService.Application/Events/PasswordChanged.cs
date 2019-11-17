using System;
using Newtonsoft.Json;
using U.EventBus.Events;

namespace U.IdentityService.Application.Events
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