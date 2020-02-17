using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.SubscriptionService.Application.Exceptions;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Command.AllowedEvents
{
    public class SetAllowedPreferencesCommandHandler : IRequestHandler<SetAllowedPreferencesCommand>
    {
        private readonly SubscriptionContext _context;

        public SetAllowedPreferencesCommandHandler(SubscriptionContext context)
        {
            _context = context;
        }


        public async Task<Unit> Handle(SetAllowedPreferencesCommand request, CancellationToken cancellationToken)
        {
            var allowedEvents = _context.UsersSubscription
                .Include(x=>x.AllowedEvents)
                .FirstOrDefault(z => z.UserId.Equals(request.UserId))
                ?.AllowedEvents;

            if (allowedEvents is null)
            {
                throw new SubscriptionNotFoundException();
            }

            _context.RemoveRange(allowedEvents);

            await _context.AddRangeAsync(request.IntegrationEventTypes.Select(x => new Domain.AllowedEvents
            {
                Allowed = x
            }), cancellationToken);

            return Unit.Value;
        }
    }
}