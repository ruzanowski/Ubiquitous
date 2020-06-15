using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.NotificationService.Application.Queries.GetCount;
using U.NotificationService.Application.Queries.GetStatistics;
using U.NotificationService.Application.Queries.GetTypesCount;

namespace U.NotificationService.Controllers
{
    /// <summary>
    /// notification controller
    /// </summary>
    [Route("api/notification/notifications")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// notification controller of notification identifierService
        /// </summary>
        /// <param name="mediator"></param>
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get notification sent times total count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCount([FromQuery] GetNotificationCount query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }

        /// <summary>
        /// Get processed notification statistics from 14 days
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/creation")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStatistics([FromQuery] GetNotificationStatistics query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }

        /// <summary>
        /// Get notification sent times total count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/event-types")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEventTypesCount([FromQuery] GetNotificationTypesCount query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
    }
}