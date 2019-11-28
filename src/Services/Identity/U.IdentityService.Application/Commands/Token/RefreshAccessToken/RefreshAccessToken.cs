using MediatR;
using U.Common.Jwt;

namespace U.IdentityService.Application.Commands.Token.RefreshAccessToken
{
    public class RefreshAccessToken : IRequest<JsonWebToken>
    {
        public string Token { get; set; }
    }
}