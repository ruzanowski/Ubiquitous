using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.SubscriptionService.Domain;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Command.Preferences
{
    public class SetPreferencesCommandHandler : IRequestHandler<SetPreferencesCommand>
    {
        private readonly SubscriptionContext _context;

        public SetPreferencesCommandHandler(SubscriptionContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetPreferencesCommand request, CancellationToken cancellationToken)
        {
            var userSubscription = _context.UserSubscriptions.FirstOrDefault(x => x.UserId.Equals(request.UserId));

            if (userSubscription is null)
            {
                userSubscription = UserSubscription.Factory.Create(request.UserId);
            }

            if (request.Preferences is null)
            {
                throw new ArgumentException("Preferences must not be null");
            }

            userSubscription.Preferences = request.Preferences;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}