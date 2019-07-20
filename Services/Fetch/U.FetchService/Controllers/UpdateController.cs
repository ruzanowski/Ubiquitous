using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace U.FetchService.Controllers
{
    public class UpdateController : Controller
    {
        private readonly IBusClient _client;
        public UpdateController(IBusClient client)
        {
            _client = client;
        }
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
        
            return Accepted();
        }
    }
}