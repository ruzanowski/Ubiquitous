using Microsoft.AspNetCore.Mvc;

namespace U.SmartStoreAdapter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/smartstore")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("health")]
        public IActionResult HealthCheck() => NoContent();
    }
}