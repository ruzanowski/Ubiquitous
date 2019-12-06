using System;
using MediatR;

namespace U.SubscriptionService.Application.Command.SignalRConnections.Bind
{
    public class BindConnectionToUserCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}