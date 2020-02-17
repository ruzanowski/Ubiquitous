using MediatR;
using U.Common.Jwt;
using U.Common.Jwt.Models;

namespace U.IdentityService.Application.Commands.Token.RefreshAccessToken
{
    public class RefreshAccessToken : IRequest<JsonWebToken>
    {
        public string Token { get; set; }
    }
}