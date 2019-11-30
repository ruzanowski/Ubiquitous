using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.NotificationService.Domain.Entities;

namespace U.SubscriptionService.Domain
{
    public class UserSubscription
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual ICollection<SignalRConnection> Connections { get; set; }
        public ICollection<AllowedEvents> AllowedEvents { get; set; }
        public Preferences Preferences { get; set; }
    }

    public class AllowedEvents
    {
        public ICollection<IntegrationEventType> Allowed { get; set; }
    }
}