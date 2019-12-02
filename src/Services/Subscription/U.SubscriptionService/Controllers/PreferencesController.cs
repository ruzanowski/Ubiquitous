using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U.Common.Jwt;
using U.Common.Jwt.Claims;
using U.Common.Subscription;
using U.SubscriptionService.Application.Command.Preferences;
using U.SubscriptionService.Application.Query;

namespace U.SubscriptionService.Controllers
{
    [Route("api/subscription/preferences")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Product controller of product service
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="contextAccessor"></param>
        public PreferencesController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("me")]
        [ProducesResponseType(typeof(Preferences), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyPreferencesAsync()
        {

            var preferences = new MyPreferencesQuery
            {
                UserId = _contextAccessor.HttpContext.GetUser().Id
            };

            var queryResult = await _mediator.Send(preferences);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("me")]
        [ProducesResponseType(typeof(Preferences), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SetPreferences(Preferences preferences)
        {
            var command = new SetPreferencesCommand
            {
                Preferences = preferences,
                UserId = _contextAccessor.HttpContext.GetUser().Id
            };

            await _mediator.Send(command);
            return Ok();
        }

    }
}