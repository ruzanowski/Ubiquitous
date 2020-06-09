using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U.Common.NetCore.Auth.Attributes;
using U.Common.NetCore.Auth.Claims;
using U.Common.Subscription;
using U.SubscriptionService.Application.Command.AllowedEvents;
using U.SubscriptionService.Application.Command.Preferences;
using U.SubscriptionService.Application.Query;

namespace U.SubscriptionService.Controllers
{
    [Route("api/subscription/preferences")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PreferencesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Product controller
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
        [JwtAuth]
        public async Task<IActionResult> GetMyPreferencesAsync()
        {
            var preferences = new MyPreferencesQuery
            {
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id
            };

            var queryResult = await _mediator.Send(preferences);
            return Ok(queryResult);
        }
        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(typeof(Preferences), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyPreferencesAsync(Guid userId)
        {
            var preferences = new MyPreferencesQuery
            {
                UserId = userId
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
        [JwtAuth]
        public async Task<IActionResult> SetPreferences(Preferences preferences)
        {
            var command = new SetPreferencesCommand
            {
                Preferences = preferences,
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id
            };

            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("me/allowed-events")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [JwtAuth]
        public async Task<IActionResult> SetAllowedEventsAsync(ISet<IntegrationEventType> allowedEvents)
        {
            var preferences = new SetAllowedPreferencesCommand
            {
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id,
                IntegrationEventTypes = allowedEvents
            };

            await _mediator.Send(preferences);
            return NoContent();
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("me/allowed-events")]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        [JwtAuth]
        public async Task<IActionResult> GetMyAllowedEvents()
        {
            var connections = new ListAllowedEventsQuery
            {
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id
            };

            var queryResult = await _mediator.Send(connections);
            return Ok(queryResult);
        }
    }
}