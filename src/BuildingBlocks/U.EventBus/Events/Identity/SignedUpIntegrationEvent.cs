using System;
using U.Common.Subscription;

namespace U.EventBus.Events.Identity
{
    public class SignedUpIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Role { get; }

        public SignedUpIntegrationEvent(Guid userId, string email, string role)
        {
            UserId = userId;
            Email = email;
            Role = role;
        }

        public override string MethodTag => nameof(SignedUpIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.SignedUp;
    }
}