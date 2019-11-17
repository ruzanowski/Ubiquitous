using Microsoft.AspNetCore.Mvc;
using U.Common.Consul;
using U.Common.Mvc;

namespace U.IdentityService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/identity")]
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