using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.SmartStoreAdapter.Application.Models.Manufacturers;

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
        public ManufacturersController(IMediator mediator)
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
            var id = await _mediator.Send(products);
            return CreatedAtAction(nameof(Store), id);
        }
    }
}