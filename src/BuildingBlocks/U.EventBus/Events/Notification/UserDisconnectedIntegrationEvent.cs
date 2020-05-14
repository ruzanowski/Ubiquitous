using System;
using U.Common.Subscription;

namespace U.EventBus.Events.Notification
{
    public class UserDisconnectedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
        public override string MethodTag => nameof(UserDisconnectedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.UserDisconnected;
    }

    public sealed class UserDisconnectedSignalRIntegrationEvent : UserDisconnectedIntegrationEvent
    {
        public override string MethodTag => nameof(UserDisconnectedSignalRIntegrationEvent);
    }

    public sealed class UserDisconnectedEmailIntegrationEvent : UserDisconnectedIntegrationEvent
    {
        public override string MethodTag => nameof(UserDisconnectedEmailIntegrationEvent);
    }
}