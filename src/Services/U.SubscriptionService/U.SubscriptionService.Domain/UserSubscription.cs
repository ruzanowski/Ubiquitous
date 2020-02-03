using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using U.Common.Subscription;

namespace U.SubscriptionService.Domain
{
    public class UserSubscription
    {
        private UserSubscription()
        {

        }

        private UserSubscription(Guid userId)
        {
            UserId = userId;
            Preferences = new Preferences();
            AllowedEvents = GetAllAllowedEvents();
            Connections = new List<SignalRConnection>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<SignalRConnection> Connections { get; set; }
        public ICollection<AllowedEvents> AllowedEvents { get; set; }
        public Preferences Preferences { get; set; }

        private IList<AllowedEvents> GetAllAllowedEvents()
        {
            var names = Enum.GetNames(typeof(IntegrationEventType));

            var allAllowedEvents = names.Select(x => new AllowedEvents
            {
                Allowed = (IntegrationEventType) Enum.Parse(typeof(IntegrationEventType), x)
            });
            return allAllowedEvents.ToList();
        }

        public void BindConnectionWithUserId(string connectionId, Guid userId)
        {
            var alreadyAdded = Connections.Any(x => x.ConnectionId.Equals(connectionId) && UserId.Equals(userId));

            if (alreadyAdded)
            {
                return;
            }

            Connections.Add(new SignalRConnection
            {
                ConnectionId = connectionId,
                UserId = userId
            });
        }

        public void UnBindConnectionWithUserId(string connectionId, Guid userId)
        {
            var bound = Connections.FirstOrDefault(x => x.ConnectionId.Equals(connectionId) && UserId.Equals(userId));

            if (bound is null)
            {
                return;
            }

            Connections.Remove(bound);
        }


        public static class Factory
        {
            public static UserSubscription Create(Guid userId) => new UserSubscription(userId);
        }
    }
}