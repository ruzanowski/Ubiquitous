using Microsoft.AspNetCore.Mvc;
using U.FetchService.Application.Models.Wholesales;

namespace U.FetchService.Controllers
{
    [ApiController]
    [Route("api/fetcher/wholesales")]
    public class WholesaleController : ControllerBase
    {
        private readonly IAvailableWholesales _wholesales;

        public WholesaleController(IAvailableWholesales wholesales)
        {
            _wholesales = wholesales;
        }
        
        [HttpGet("settings")]
        public IActionResult GetSettings()
        {
            return Ok(_wholesales);
        }
    }
}