using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.Queries.GetCount
{
    public class GetNotificationCountQueryHandler : IRequestHandler<GetNotificationCount, int>
    {
        private readonly NotificationContext _context;

        public GetNotificationCountQueryHandler(NotificationContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetNotificationCount request, CancellationToken cancellationToken)
        {
            var products = GetQuery();

            var count = await products.SumAsync(x => x.TimesSent, cancellationToken);

            return count;
        }

        private IQueryable<Notification> GetQuery() => _context.Notifications
            .AsQueryable();

    }
}
