using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U.Common.Jwt;
using U.Common.Jwt.Attributes;
using U.Common.Jwt.Claims;
using U.SubscriptionService.Application.Command;
using U.SubscriptionService.Application.Command.SignalRConnections.Bind;
using U.SubscriptionService.Application.Command.SignalRConnections.Unbind;
using U.SubscriptionService.Application.Query;

namespace U.SubscriptionService.Controllers
{
    [Route("api/subscription/signalr")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Product controller of product service
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="contextAccessor"></param>
        public SignalRController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/bind")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> BindAsync(Guid userId, string connectionId)
        {
            var preferences = new BindConnectionToUserCommand
            {
                UserId = userId,
                ConnectionId = connectionId
            };

            await _mediator.Send(preferences);
            return NoContent();
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/unbind")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> UnbindAsync(Guid userId, string connectionId)
        {

            var preferences = new UnbindConnectionToUserCommand
            {
                UserId = userId,
                ConnectionId = connectionId
            };

            await _mediator.Send(preferences);

            return NoContent();
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetConnections()
        {
            var connections = new ListSignalRConnectionQuery
            {
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id,
            };

            var queryResult = await _mediator.Send(connections);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("me/list")]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyConnections()
        {
            var connections = new ListSignalRConnectionQuery();

            var queryResult = await _mediator.Send(connections);
            return Ok(queryResult);
        }


        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("me/bind")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [JwtAuth]
        public async Task<IActionResult> BindMeAsync(string connectionId)
        {

            var preferences = new BindConnectionToUserCommand
            {
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id,
                ConnectionId = connectionId
            };

            await _mediator.Send(preferences);
            return NoContent();
        }

        /// <summary>
        /// Get my preferences
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("me/unbind")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [JwtAuth]
        public async Task<IActionResult> UnbindMeAsync(string connectionId)
        {

            var preferences = new UnbindConnectionToUserCommand
            {
                UserId = _contextAccessor.HttpContext.GetUserOrThrow().Id,
                ConnectionId = connectionId
            };

            await _mediator.Send(preferences);

            return NoContent();
        }
    }
}