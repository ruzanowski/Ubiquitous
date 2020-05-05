using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.SmartStoreAdapter.Application.Products;

namespace U.SmartStoreAdapter.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/smartstore/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mediator"></param>
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of products
        /// </summary>
        /// <param name="productsListQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedItems<SmartProductViewModel>), 200)]
        public async Task<IActionResult> GetProductsList([FromQuery] GetProductsListQuery productsListQuery)
        {
            var products = await _mediator.Send(productsListQuery);
            return Ok(products);
        }

        /// <summary>
        /// Get product by its id or systemName
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SmartProductViewModel), 200)]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductQuery productQuery)
        {
            var products = await _mediator.Send(productQuery);
            return Ok(products);
        }

        /// <summary>
        /// Add or update Product
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("store")]
        [ProducesResponseType(typeof(int), 201)]
        public async Task<IActionResult> StoreProducts([FromBody] StoreProductsCommand products)
        {
            var id = await _mediator.Send(products);
            return CreatedAtAction(nameof(GetProduct), new {id}, id);
        }
    }
}