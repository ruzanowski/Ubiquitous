using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using U.Common.NetCore.Consul;

namespace U.ApiGateway.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/apigateway")]
    [ExcludeFromCodeCoverage]
    public class HealthController : ControllerBase
    {
        private readonly IConsulServiceDifferentator _service;

        public HealthController(IConsulServiceDifferentator service)
        {
            _service = service;
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

        private bool IsCorrectServiceId(string guid) => _service.IsTheSame(guid);
    }
}