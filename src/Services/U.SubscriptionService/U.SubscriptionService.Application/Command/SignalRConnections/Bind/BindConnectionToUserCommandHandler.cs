using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.SubscriptionService.Domain;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Application.Command.SignalRConnections.Bind
{
    public class BindConnectionToUserCommandHandler : IRequestHandler<BindConnectionToUserCommand>
    {
        private readonly SubscriptionContext _context;

        public BindConnectionToUserCommandHandler(SubscriptionContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BindConnectionToUserCommand request, CancellationToken cancellationToken)
        {
            var userSubscription = _context.UsersSubscription
                .Include(x=>x.Preferences)
                .Include(x=>x.Connections)
                .Include(x=>x.AllowedEvents)
                .FirstOrDefault(x => x.UserId.Equals(request.UserId));

            if (userSubscription is null)
            {
                userSubscription = UserSubscription.Factory.Create(request.UserId);
                _context.Add(userSubscription);
            }

            userSubscription.BindConnectionWithUserId(request.ConnectionId, request.UserId);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}