using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using U.Common.Jwt;
using U.IdentityService.Application.Commands.Identity.ChangePassword;
using U.IdentityService.Application.Commands.Identity.SignIn;
using U.IdentityService.Application.Commands.Identity.SignUp;

namespace U.IdentityService.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : IdentifiedBaseController
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([Required] [FromQuery] SignUp command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([Required] [FromQuery] SignIn command)
        {
            JsonWebToken jwt = await _mediator.Send(command);

            return Ok(jwt);
        }

        [HttpGet("me")]
        [JwtAuth]
        public IActionResult Get() => Content($"Your id: '{UserId:N}'.");

        [HttpPut("me/password")]
        [JwtAuth]
        public async Task<ActionResult> ChangePassword([Required] [FromQuery] ChangePasswordDto command)
        {
            var changePassword = new ChangePassword()
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