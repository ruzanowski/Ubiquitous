using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using U.FetchService.Api;

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
            await _client.PublishAsync(new SendMessage("Test Message"));
        
            return Accepted();
        }
    }
}