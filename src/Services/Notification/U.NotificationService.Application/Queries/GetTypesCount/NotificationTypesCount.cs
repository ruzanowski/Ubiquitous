using U.Common.Subscription;

namespace U.NotificationService.Application.Queries.GetTypesCount
{
    public class NotificationTypesCount
    {
        public IntegrationEventType EventType { get; set; }
        public int Count { get; set; }
    }
}