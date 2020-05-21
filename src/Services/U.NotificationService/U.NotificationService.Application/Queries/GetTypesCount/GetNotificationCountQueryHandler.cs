using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.Queries.GetTypesCount
{
    public class GetNotificationTypesCountQueryHandler : IRequestHandler<GetNotificationTypesCount, IEnumerable<NotificationTypesCount>>
    {
        private readonly NotificationContext _context;

        public GetNotificationTypesCountQueryHandler(NotificationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationTypesCount>> Handle(GetNotificationTypesCount request, CancellationToken cancellationToken)
        {
            var notifications = GetQuery();

            var groupedTypes = notifications
                .GroupBy(g => new
                {
                    g.IntegrationEventType
                })
                .Select(s => new
                {
                    s.Key.IntegrationEventType,
                    Count = s.Count()
                });

            var eventTypes = groupedTypes.Select(x => new NotificationTypesCount
            {
                EventType = x.IntegrationEventType,
                Count = x.Count
            });

            return eventTypes;
        }

        private IQueryable<Notification> GetQuery() => _context.Notifications
            .AsQueryable();
    }
}