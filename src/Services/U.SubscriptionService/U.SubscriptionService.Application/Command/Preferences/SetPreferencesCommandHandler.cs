using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.SubscriptionService.Application.Exceptions;
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
            if (request.Preferences is null)
            {
                throw new ArgumentException("Preferences must not be null");
            }

            var userSubscription = _context.UsersSubscription
                .Include(x=>x.Preferences)
                .Include(x=>x.Connections)
                .Include(x=>x.AllowedEvents)
                .FirstOrDefault(x => x.UserId.Equals(request.UserId));

            if (userSubscription is null)
            {
                throw new SubscriptionNotFoundException();
            }

            userSubscription.Preferences = request.Preferences;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}