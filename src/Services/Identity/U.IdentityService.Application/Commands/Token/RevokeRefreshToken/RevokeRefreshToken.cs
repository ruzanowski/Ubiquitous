using System;
using MediatR;

namespace U.IdentityService.Application.Commands.Token.RevokeRefreshToken
{
    public class RevokeRefreshToken : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}