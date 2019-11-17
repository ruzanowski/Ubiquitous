using MediatR;
using U.Common.Jwt;

namespace U.IdentityService.Application.Commands.Identity.SignIn
{
    public class SignIn : IRequest<JsonWebToken>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}