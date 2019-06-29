using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using U.SmartStoreAdapter.Api.Manufacturers;

namespace U.SmartStoreAdapter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/smartstore/manufacturers")]
    public class ManufacturersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public ManufacturersController(IMediator mediator, ILogger<ManufacturersController> logger)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("store")]
        [ProducesResponseType(typeof(ManufacturerViewModel), 201)]
        public async Task<IActionResult> Store([FromBody] StoreManufacturerCommand products)
        {
            var result = await _mediator.Send(products);
            return CreatedAtAction(nameof(Store), result);
        }
    }
}