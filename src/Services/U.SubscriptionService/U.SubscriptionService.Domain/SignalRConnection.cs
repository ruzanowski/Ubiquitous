using System;

namespace U.SubscriptionService.Domain
{
    public class SignalRConnection
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; } // composite key
    }
}