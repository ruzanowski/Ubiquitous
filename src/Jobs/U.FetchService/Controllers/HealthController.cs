using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using U.Common.NetCore.Consul;

namespace U.FetchService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/fetcher")]
    [ExcludeFromCodeCoverage]
    public class HealthController : ControllerBase
    {
        private readonly IConsulIdentifierService _identifierService;

        public HealthController(IConsulIdentifierService identifierService)
        {
            _identifierService = identifierService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet("health/{serviceId}")]
        public IActionResult HealthCheck(string serviceId)
        {
            return IsCorrectServiceId(serviceId) ? (IActionResult) NoContent() : BadRequest();
        }

        private bool IsCorrectServiceId(string guid) => _identifierService.IsTheSame(guid);
    }
}