using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U.Common.Consul;

namespace U.ProductService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/product")]
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
        [AllowAnonymous]
        [HttpGet("health/{serviceId}")]
        public IActionResult HealthCheck(string serviceId)
        {
            return IsCorrectServiceId(serviceId) ? (IActionResult) NoContent() : BadRequest();
        }

        private bool IsCorrectServiceId(string guid) => _service.IsTheSame(guid);
    }
}