using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Query
{
    public class ListSignalRConnectionsQueryHandler : IRequestHandler<ListSignalRConnectionQuery, IList<string>>
    {
        private readonly SubscriptionContext _context;
        private readonly IMapper _mapper;

        public ListSignalRConnectionsQueryHandler(SubscriptionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<string>> Handle(ListSignalRConnectionQuery request, CancellationToken cancellationToken)
        {
            var signalrConnection = await _context.SignalRConnections
                .Where(x => x.UserId.Equals(request.UserId))
                .Select(x=>x.ConnectionId)
                .ToListAsync(cancellationToken);

            return signalrConnection;
        }
    }
}