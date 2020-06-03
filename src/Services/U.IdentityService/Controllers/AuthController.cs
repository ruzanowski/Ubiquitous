using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using U.Common.NetCore.Auth.Attributes;
using U.Common.NetCore.Auth.Models;
using U.IdentityService.Application.Commands.Identity.ChangePassword;
using U.IdentityService.Application.Commands.Identity.SignIn;
using U.IdentityService.Application.Commands.Identity.SignUp;

namespace U.IdentityService.Controllers
{
    [Route("api/identity/auth")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class AuthController : IdentifiedBaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([Required] [FromBody] SignUp command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([Required] [FromBody] SignIn command)
        {
            JsonWebToken jwt = await _mediator.Send(command);

            return Ok(jwt);
        }

        [HttpPut("password")]
        [JwtAuth]
        public async Task<ActionResult> ChangePasswordAsync([Required] [FromQuery] ChangePasswordDto command)
        {
            var changePassword = new ChangePassword
            {
                UserId = UserId,
                CurrentPassword = command.CurrentPassword,
                NewPassword = command.NewPassword
            };

            await _mediator.Send(changePassword);

            return NoContent();
        }
    }
}