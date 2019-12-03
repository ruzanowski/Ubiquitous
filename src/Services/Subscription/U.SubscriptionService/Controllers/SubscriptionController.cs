using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U.Common.Jwt.Claims;
using U.NotificationService.Domain.Entities;
using U.SubscriptionService.Application.Command.AllowedEvents;
using U.SubscriptionService.Application.Command.SignalRConnections.Bind;
using U.SubscriptionService.Application.Command.SignalRConnections.Unbind;
using U.SubscriptionService.Application.Query;

namespace U.SubscriptionService.Controllers
{
    [Route("api/subscription/subscription")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Product controller of product service
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="contextAccessor"></param>
        public SubscriptionController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("allowed-events")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> SetAllowedEventsAsync(ISet<IntegrationEventType> allowedEvents)
        {

            var preferences = new SetAllowedPreferencesCommand
            {
                UserId = _contextAccessor.HttpContext.GetUser().Id,
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
        [Route("current-user-list")]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyAllowedEvents()
        {
            var connections = new ListAllowedEventsQuery
            {
                UserId = _contextAccessor.HttpContext.GetUser().Id
            };

            var queryResult = await _mediator.Send(connections);
            return Ok(queryResult);
        }
    }
}
