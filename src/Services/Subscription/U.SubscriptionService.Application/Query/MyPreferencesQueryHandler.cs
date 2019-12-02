using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.Common.Subscription;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Query
{
    public class MyPreferencesQueryHandler : IRequestHandler<MyPreferencesQuery, Preferences>
    {
        private readonly SubscriptionContext _context;
        private readonly IMapper _mapper;

        public MyPreferencesQueryHandler(SubscriptionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Preferences> Handle(MyPreferencesQuery request, CancellationToken cancellationToken)
        {
            var userSubscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(x => x.UserId.Equals(request.UserId),
                    cancellationToken);

            var preferences = userSubscription?.Preferences ?? new Preferences();

            return preferences;
        }
    }
}