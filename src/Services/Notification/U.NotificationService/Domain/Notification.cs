using System;
using System.Collections.Generic;
using U.EventBus.Events;

namespace U.NotificationService.Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public IntegrationEvent IntegrationEvent { get; set; }
        public IEnumerable<Confirmation> Confirmations { get; set; }
    }
}