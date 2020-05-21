using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U.Common.Jwt.Attributes;
using U.IdentityService.Application.Commands.Token.RefreshAccessToken;
using U.IdentityService.Application.Commands.Token.RevokeAccessToken;
using U.IdentityService.Application.Commands.Token.RevokeRefreshToken;

namespace U.IdentityService.Controllers
{

    [Route("api/identity/token")]
    [ApiController]
    public class TokensController : IdentifiedBaseController
    {
        private readonly IMediator _mediator;

        public TokensController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("access/{refreshToken}/refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            var refreshAccess = new RefreshAccessToken
            {
                Token = refreshToken
            };

            var refreshedAccess = await _mediator.Send(refreshAccess);

            return Ok(refreshedAccess);
        }

        [HttpPost("access/revoke")]
        [JwtAuth]
        public async Task<IActionResult> RevokeAccessToken()
        {
            var revokeAccess = new RevokeAccessToken();

            await _mediator.Send(revokeAccess);

            return NoContent();
        }

        [HttpPost("refresh/{refreshToken}/revoke")]
        [JwtAuth]
        public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
        {
            var revokeRefresh = new RevokeRefreshToken
            {
                Token = refreshToken,
                UserId = UserId
            };

            await _mediator.Send(revokeRefresh);

            return NoContent();
        }
    }
}