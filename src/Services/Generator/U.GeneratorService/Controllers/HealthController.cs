using Microsoft.AspNetCore.Mvc;
using U.Common.Mvc;

namespace U.GeneratorService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/generator")]
    public class HealthController : ControllerBase
    {
        private readonly IServiceIdService _service;

        public HealthController(IServiceIdService service)
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

        private bool IsCorrectServiceId(string guid) => _service.Id.Equals(guid);
    }
}