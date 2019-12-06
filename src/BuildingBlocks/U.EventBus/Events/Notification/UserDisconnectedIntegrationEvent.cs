using System;

namespace U.EventBus.Events.Notification
{
    public class UserDisconnectedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
    }

    public sealed class UserDisconnectedSignalRIntegrationEvent : UserDisconnectedIntegrationEvent
    {

    }

    public sealed class UserDisconnectedEmailIntegrationEvent : UserDisconnectedIntegrationEvent
    {

    }
}