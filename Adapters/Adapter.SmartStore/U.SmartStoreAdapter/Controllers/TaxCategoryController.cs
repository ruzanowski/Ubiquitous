using System.Threading.Tasks;
using U.SmartStoreAdapter.Application.Operations.TaxCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.SmartStoreAdapter.Api.Products;
using U.SmartStoreAdapter.Api.Taxes;

namespace U.SmartStoreAdapter.Controllers
{
    /// <summary>
    /// Tax Category
    /// </summary>
    [ApiController]
    [Route("api/smartstore/taxcategory")]
    public class TaxCategoryController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public TaxCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("store")]
        [ProducesResponseType(typeof(TaxResponse), 201)]
        public async Task<IActionResult> StoreTaxCategory([FromQuery] StoreTaxCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(StoreTaxCategory), result);
        }
    }
}