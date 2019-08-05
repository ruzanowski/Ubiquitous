using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.CreateProduct;
using U.ProductService.Application.Products.Commands.UpdateProduct;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.QueryProduct;
using U.ProductService.Application.Products.Queries.QueryProductByAlternativeKey;
using U.ProductService.Application.Products.Queries.QueryProducts;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// Product controller of product service
    /// </summary>
    [Route("api/product-service/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Product controller of product service
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
        [Route("query-list")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), 200)]
        public async Task<IActionResult> GetProductsList([FromQuery] GetProductsListQuery productsListQuery)
        {
            var queryResult = await _mediator.Send(productsListQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by its id 
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query/{Id:Guid}")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), 200)]
        public async Task<IActionResult> GetProduct([FromRoute] QueryProduct productQuery)
        {
            var queryResult = await _mediator.Send(productQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by alternate key
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query-alt-key/{AlternativeKey}")]
        [ProducesResponseType(typeof(ProductViewModel), 200)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct([FromRoute] QueryProductByAlternativeKey productQuery)
        {
            var queryResult = await _mediator.Send(productQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Guid), 201)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand products)
        {
            var commandResult = await _mediator.Send(products);
            return CreatedAtAction(nameof(CreateProduct), commandResult);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{productId:Guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductCommand products)
        {
            var commandResult = await _mediator.Send(products.BindProductId(productId));
            return CreatedAtAction(nameof(UpdateProduct), commandResult);
        }
    }
}