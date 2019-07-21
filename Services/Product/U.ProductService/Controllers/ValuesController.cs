using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.ProductService.Application.Commands;

namespace U.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}