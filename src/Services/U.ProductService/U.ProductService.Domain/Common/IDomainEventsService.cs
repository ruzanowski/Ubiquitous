using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace U.ProductService.Domain.Common
{
    public interface IDomainEventsService
    {
        IList<INotification> GetDomainEvents();
        void AddDomainEvents(IEnumerable<INotification> eventItems);
        void RemoveDomainEvent(INotification eventItem);
        void ClearDomainEvents();
    }

    public class DomainEventsService : IDomainEventsService
    {
        private readonly ICollection<INotification> _notifications = new HashSet<INotification>();

        public IList<INotification> GetDomainEvents() => _notifications.ToList();

        public void AddDomainEvents(IEnumerable<INotification> eventItems)
        {
            foreach (var notification in eventItems)
            {
                _notifications.Add(notification);
            }
        }

        public void RemoveDomainEvent(INotification eventItem) => _notifications.Remove(eventItem);

        public void ClearDomainEvents() => _notifications.Clear();
    }
}