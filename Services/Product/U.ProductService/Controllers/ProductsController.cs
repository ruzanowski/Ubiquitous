using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.AddPicture;
using U.ProductService.Application.Products.Commands.ChangePrice;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.DeletePicture;
using U.ProductService.Application.Products.Commands.Publish;
using U.ProductService.Application.Products.Commands.UnPublish;
using U.ProductService.Application.Products.Commands.Update;
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
        [Route("query")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsListAsync([FromQuery] GetProductsListQuery productsListQuery)
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
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductAsync([FromRoute] QueryProduct productQuery)
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
        [ProducesResponseType(typeof(ProductViewModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductAsync([FromRoute] QueryProductByAlternativeKey productQuery)
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
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand products)
        {
            var productId = await _mediator.Send(products);
            return CreatedAtAction(nameof(CreateProductAsync), new {productId});
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{productId:Guid}")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productId,
            [FromBody] UpdateProductCommand products)
        {
            await _mediator.Send(products.BindProductId(productId));
            return Ok();
        }

        /// <summary>
        /// Publish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("publish/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> PublishProductAsync([FromRoute] Guid productId)
        {
            await _mediator.Send(new PublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// UnPublish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("unpublish/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UnPublishProductAsync([FromRoute] Guid productId)
        {
            await _mediator.Send(new UnPublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// Change product price
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("change-price/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductPriceAsync([FromRoute] Guid productId, [FromQuery] decimal price)
        {
            await _mediator.Send(new ChangeProductPriceCommand(productId, price));
            return Ok();
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("add-picture/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> AddPictureAsync([FromRoute] Guid productId,
            [FromQuery] AddProductPictureCommand command)
        {
            var pictureId = await _mediator.Send(command.BindProductId(productId));
            return Ok(new {pictureId});
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-picture/{productId:Guid}/{pictureId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePictureAsync([FromRoute] Guid productId, [FromRoute] Guid pictureId)
        {
            await _mediator.Send(new DeleteProductPictureCommand(productId, pictureId));
            return Ok();
        }
    }
}