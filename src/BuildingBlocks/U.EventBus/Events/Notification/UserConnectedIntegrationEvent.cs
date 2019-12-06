using System;

namespace U.EventBus.Events.Notification
{
    public class UserConnectedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
    }

    public sealed class UserConnectedSignalRIntegrationEvent : UserConnectedIntegrationEvent
    {

    }

    public sealed class UserConnectedEmailIntegrationEvent : UserConnectedIntegrationEvent
    {

    }
}