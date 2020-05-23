using MediatR;
using U.Common.NetCore.Auth.Models;

namespace U.IdentityService.Application.Commands.Token.RefreshAccessToken
{
    public class RefreshAccessToken : IRequest<JsonWebToken>
    {
        public string Token { get; set; }
    }
}