using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands;
using U.ProductService.Application.Products.Commands.AddPicture;
using U.ProductService.Application.Products.Commands.ChangeCategory;
using U.ProductService.Application.Products.Commands.ChangePrice;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.DeletePicture;
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
    [Route("api/product/products")]
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
        /// Get list of command 
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
        /// Get product by its productId 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query/{productId:Guid}")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductAsync([FromRoute] Guid productId)
        {
            var queryResult = await _mediator.Send(new QueryProduct(productId));
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
            return CreatedAtAction(nameof(CreateProductAsync), productId);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{ProductId}")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProductAsync([FromQuery] UpdateProductCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Publish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("publish/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
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
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("change-price/{ProductId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductPriceAsync([FromQuery] ChangeProductPriceCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("add-picture/{ProductId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddPictureAsync([FromQuery] AddProductPictureCommand command)
        {
            var pictureId = await _mediator.Send(command);
            return Ok(pictureId);
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-picture/{productId:Guid}/{pictureId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePictureAsync([FromRoute] DeleteProductPictureCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics")]
        [ProducesResponseType(typeof(ProductStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStatisticsAsync([FromQuery] GetProductsStatisticsQuery command)
        {
            var statistics = await _mediator.Send(command);
            return Ok(statistics);
        }

        /// <summary>
        /// Change product category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("change-category/{ProductId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductCategoryAsync([FromQuery] ChangeProductCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}