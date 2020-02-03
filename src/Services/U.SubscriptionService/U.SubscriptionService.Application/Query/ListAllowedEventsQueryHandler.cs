using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.SubscriptionService.Application.Exceptions;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Query
{
    public class ListAllowedEventsQueryHandler : IRequestHandler<ListAllowedEventsQuery, IList<string>>
    {
        private readonly SubscriptionContext _context;

        public ListAllowedEventsQueryHandler(SubscriptionContext context)
        {
            _context = context;
        }


        public async Task<IList<string>> Handle(ListAllowedEventsQuery request, CancellationToken cancellationToken)
        {
            var allowedEvents = _context.UsersSubscription
                .Include(x=>x.AllowedEvents)
                .FirstOrDefault(z => z.UserId.Equals(request.UserId))
                ?.AllowedEvents
                .ToList();

            if (allowedEvents is null)
            {
                throw new SubscriptionNotFoundException();
            }

            var enumerable = allowedEvents.Select(x => x.Allowed.ToString()).ToList();
            await Task.CompletedTask;

            return enumerable;
        }

    }
}