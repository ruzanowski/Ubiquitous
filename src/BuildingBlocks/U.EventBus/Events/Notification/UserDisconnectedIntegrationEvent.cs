using System;

namespace U.EventBus.Events.Notification
{
    public class UserDisconnected : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
    }
}