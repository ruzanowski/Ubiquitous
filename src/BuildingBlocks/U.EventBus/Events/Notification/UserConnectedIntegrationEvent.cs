using System;
using U.Common.Subscription;

namespace U.EventBus.Events.Notification
{
    public class UserConnectedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
        public override string MethodTag => nameof(UserConnectedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.UserConnected;
    }

    public sealed class UserConnectedSignalRIntegrationEvent : UserConnectedIntegrationEvent
    {
        public override string MethodTag => nameof(UserConnectedSignalRIntegrationEvent);
    }

    public sealed class UserConnectedEmailIntegrationEvent : UserConnectedIntegrationEvent
    {
        public override string MethodTag => nameof(UserConnectedEmailIntegrationEvent);
    }
}