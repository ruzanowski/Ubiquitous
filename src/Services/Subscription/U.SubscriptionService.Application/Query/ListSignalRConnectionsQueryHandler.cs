using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Query
{
    public class ListSignalRConnectionsQueryHandler : IRequestHandler<ListSignalRConnectionQuery, IList<string>>
    {
        private readonly SubscriptionContext _context;

        public ListSignalRConnectionsQueryHandler(SubscriptionContext context)
        {
            _context = context;
        }

        public async Task<IList<string>> Handle(ListSignalRConnectionQuery request, CancellationToken cancellationToken)
        {
            var connections = _context.SignalRConnections.AsQueryable();

            if (request.UserId is null)
            {
                connections = connections.Where(x => x.UserId.Equals(request.UserId));
            }

            return await connections
                .Select(x => x.ConnectionId)
                .ToListAsync(cancellationToken);
        }
    }
}