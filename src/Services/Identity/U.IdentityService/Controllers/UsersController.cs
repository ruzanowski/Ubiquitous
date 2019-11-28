using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using U.Common.Jwt;
using U.IdentityService.Application.Commands.Identity.ChangePassword;
using U.IdentityService.Application.Queries.GetMyProfile;
using U.IdentityService.Application.Queries.GetUsersProfiles;

namespace U.IdentityService.Controllers
{
    [Route("api/identity/users")]
    [ApiController]
    public class UsersController : IdentifiedBaseController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        [JwtAuth]
        public async Task<IActionResult> GetAsync()
        {
            var query = new GetMyProfile
            {
                UserId = UserId
            };

            var users = await _mediator.Send(query);

            return Ok(users);
        }

        [HttpGet("all")]
        [JwtAuth(Roles = "admin")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _mediator.Send(new GetUsersProfiles());

            return Ok(users);
        }

        [HttpPut("me/password")]
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