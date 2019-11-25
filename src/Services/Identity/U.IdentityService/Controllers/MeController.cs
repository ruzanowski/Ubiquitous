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
    [Route("api/identity/me")]
    [ApiController]
    public class MeController : IdentifiedBaseController
    {
        private readonly IMediator _mediator;

        public MeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        [JwtAuth]
        public IActionResult Get() => Content($"Your id: '{UserId:N}'.");

        [HttpPut("me/password")]
        [JwtAuth]
        public async Task<ActionResult> ChangePassword([Required] [FromQuery] ChangePasswordDto command)
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