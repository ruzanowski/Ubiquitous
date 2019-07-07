using Microsoft.AspNetCore.Mvc;
using U.FetchService.Application.Models.SubscribedServices;

namespace U.FetchService.Controllers
{
    [ApiController]
    [Route("api/fetcher/services")]
    public class ServicesController: ControllerBase
    {
        private readonly ISubscribedService _service;

        public ServicesController(ISubscribedService service)
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