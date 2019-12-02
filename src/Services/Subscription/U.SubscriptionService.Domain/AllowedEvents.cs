using U.NotificationService.Domain.Entities;

namespace U.SubscriptionService.Domain
{
    public class AllowedEvents
    {
        public int Id { get; set; }
        public IntegrationEventType Allowed { get; set; }
    }
}