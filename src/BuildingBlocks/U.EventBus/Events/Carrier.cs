using U.NotificationService.Domain.Entities;

namespace U.EventBus.Events
{
    public class Carrier<T> where T: IntegrationEvent
    {
        public T IntegrationEventPayload { get; set; }
        public Importancy? Importancy { get; set; }
        public IntegrationEventType IntegrationEventType { get; set; }
        public RouteType RouteType { get; set; }
    }
}