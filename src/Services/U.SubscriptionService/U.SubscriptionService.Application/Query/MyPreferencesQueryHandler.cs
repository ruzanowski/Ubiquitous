using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.Common.Subscription;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Query
{
    public class MyPreferencesQueryHandler : IRequestHandler<MyPreferencesQuery, Preferences>
    {
        private readonly SubscriptionContext _context;

        public MyPreferencesQueryHandler(SubscriptionContext context)
        {
            _context = context;
        }

        public async Task<Preferences> Handle(MyPreferencesQuery request, CancellationToken cancellationToken)
        {
            var userSubscription = await _context.UsersSubscription
                .Include(x=>x.Preferences)
                .FirstOrDefaultAsync(x => x.UserId.Equals(request.UserId),
                    cancellationToken);

            var preferences = userSubscription?.Preferences ?? new Preferences();

            return preferences;
        }
    }
}