using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using U.Common.NetCore.Auth.Attributes;
using U.IdentityService.Application.Queries.GetMyProfile;
using U.IdentityService.Application.Queries.GetUsersAccounts;
using U.IdentityService.Application.Queries.GetUsersOnline;

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
            var query = new GetMyProfile(UserId);

            var users = await _mediator.Send(query);

            return Ok(users);
        }

        [HttpGet("all")]
        [JwtAuth]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _mediator.Send(new GetUsersProfiles());

            return Ok(users);
        }

        [HttpGet("online")]
        [JwtAuth]
        public async Task<IActionResult> GetUsersOnlineAsync()
        {
            var online = await _mediator.Send(new GetUsersOnline());

            return Ok(online);
        }

    }
}