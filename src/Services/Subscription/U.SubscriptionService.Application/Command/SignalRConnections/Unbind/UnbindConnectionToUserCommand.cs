using System;
using MediatR;

namespace U.SubscriptionService.Application.Command.SignalRConnections.Unbind
{
    public class UnbindConnectionToUserCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}