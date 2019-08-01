using Microsoft.AspNetCore.Mvc;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/product")]
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