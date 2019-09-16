using Microsoft.AspNetCore.Mvc;

namespace U.GeneratorService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/fakeProductsGenerator")]
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