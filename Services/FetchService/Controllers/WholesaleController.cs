using System.Net.Http;
using System.Threading.Tasks;
using U.FetchService.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace U.FetchService.Controllers
{
    [ApiController]
    [Route("api/productmanager")]
    public class WholesaleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WholesaleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("start")]
        public async Task<IActionResult> Start()
        {
           var products = await _mediator.Send(new FlowManager());
           return Ok(products);
        }
    }

    
}