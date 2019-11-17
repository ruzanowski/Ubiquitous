using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using U.Common.Authentication;
using U.IdentityService.Application.Commands;
using U.IdentityService.Application.Services;

namespace U.IdentityService.Controllers
{
    [Route("")]
    [ApiController]
    public class IdentityController : IdentifiedBaseController
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet("me")]
        [JwtAuth]
        public IActionResult Get() => Content($"Your id: '{UserId:N}'.");

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUp command)
        {
            await _identityService.SignUpAsync(
                Guid.NewGuid(),
                command.Email,
                command.Password,
                command.Role);

            return NoContent();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignIn command)
            => Ok(await _identityService.SignInAsync(command.Email, command.Password));

        [HttpPut("me/password")]
        [JwtAuth]
        public async Task<ActionResult> ChangePassword(ChangePassword command)
        {
            await _identityService.ChangePasswordAsync(
                UserId,
                command.CurrentPassword,
                command.NewPassword);

            return NoContent();
        }
    }
}