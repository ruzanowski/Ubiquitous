using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.CreateProduct;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.QueryProduct;
using U.ProductService.Application.Products.Queries.QueryProductByAlternativeKey;
using U.ProductService.Application.Products.Queries.QueryProducts;

namespace U.ProductService.Controllers
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
        [Route("get-list")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), 200)]
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
        [Route("get")]
        [ProducesResponseType(typeof(ProductViewModel), 200)]
        public async Task<IActionResult> GetProduct([FromRoute] QueryProduct productQuery)
        {
            var products = await _mediator.Send(productQuery);
            return Ok(products);
        }
        
        /// <summary>
        /// Get product by its id or systemName
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-alternativeKey")]
        [ProducesResponseType(typeof(ProductViewModel), 200)]
        public async Task<IActionResult> GetProduct([FromQuery] QueryProductByAlternativeKey productQuery)
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
        [Route("create")]
        [ProducesResponseType(typeof(int), 201)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand products)
        {
            var result = await _mediator.Send(products);
            return CreatedAtAction(nameof(CreateProduct), result);
        }
    }
}