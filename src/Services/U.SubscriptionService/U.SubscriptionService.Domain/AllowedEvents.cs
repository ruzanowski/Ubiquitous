using U.Common.Subscription;

namespace U.SubscriptionService.Domain
{
    public class AllowedEvents
    {
        public int Id { get; set; }
        public IntegrationEventType Allowed { get; set; } //TODO: IMPORTANCY PER INTEGRATION EVENT, NOT GLOBAL
    }
}