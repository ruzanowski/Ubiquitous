using Microsoft.AspNetCore.Mvc;
using U.FetchService.Application.Models.SubscribedServices;

namespace U.FetchService.Controllers
{
    [ApiController]
    [Route("api/fetcher/daemon")]
    public class DaemonController: ControllerBase
    {
        private readonly ISubscribedService _service;

        public DaemonController(ISubscribedService service)
        {
            _service = service;
        }
        
        [HttpGet("settings")]
        public IActionResult GetSettings()
        {
            return Ok(_service);
        }
    }
}