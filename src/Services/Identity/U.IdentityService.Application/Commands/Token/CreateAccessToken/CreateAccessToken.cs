using System;
using MediatR;

namespace U.IdentityService.Application.Commands.Token.CreateAccessToken
{
    public class CreateAccessToken : IRequest
    {
        public Guid UserId { get; set; }

        public CreateAccessToken(Guid userId)
        {
            this.UserId = userId;
        }
    }
}