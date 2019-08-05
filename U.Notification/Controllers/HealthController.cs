using Microsoft.AspNetCore.Mvc;

namespace U.Notification.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/notification")]
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