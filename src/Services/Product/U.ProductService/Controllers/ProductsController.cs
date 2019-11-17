using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Jwt;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.AddPicture;
using U.ProductService.Application.Products.Commands.ChangeCategory;
using U.ProductService.Application.Products.Commands.ChangePrice;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.DeletePicture;
using U.ProductService.Application.Products.Commands.Publish;
using U.ProductService.Application.Products.Commands.UnPublish;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.GetList;
using U.ProductService.Application.Products.Queries.GetSingle;
using U.ProductService.Application.Products.Queries.GetSingleByAlternativeKey;
using U.ProductService.Application.Products.Queries.GetStatistics;

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
        [JwtAuth]
        [HttpGet]
        [Route("query")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsList([FromQuery] GetProductsListQuery productsListQuery)
        {
            var queryResult = await _mediator.Send(productsListQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by its productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpGet]
        [Route("query/{productId:Guid}")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId)
        {
            var queryResult = await _mediator.Send(new QueryProduct(productId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by alternate key
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpGet]
        [Route("query-alt-key/{AlternativeKey}")]
        [ProducesResponseType(typeof(ProductViewModel), (int) HttpStatusCode.OK)]
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
        [JwtAuth]
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand products)
        {
            var productId = await _mediator.Send(products);
            return CreatedAtAction(nameof(CreateProduct), productId);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpPut]
        [Route("update/{ProductId}")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct([FromQuery] UpdateProductCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Publish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpPut]
        [Route("publish/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> PublishProduct([FromRoute] Guid productId)
        {
            await _mediator.Send(new PublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// UnPublish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpPut]
        [Route("unpublish/{productId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UnPublishProduct([FromRoute] Guid productId)
        {
            await _mediator.Send(new UnPublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// Change product price
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpPut]
        [Route("change-price/{ProductId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductPrice([FromQuery] ChangeProductPriceCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpPut]
        [Route("add-picture/{ProductId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddPicture([FromQuery] AddProductPictureCommand command)
        {
            var pictureId = await _mediator.Send(command);
            return Ok(pictureId);
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpDelete]
        [Route("delete-picture/{productId:Guid}/{pictureId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePicture([FromRoute] DeleteProductPictureCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpGet]
        [Route("statistics")]
        [ProducesResponseType(typeof(ProductStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetStatistics([FromQuery] GetProductsStatisticsQuery command)
        {
            var statistics = await _mediator.Send(command);
            return Ok(statistics);
        }

        /// <summary>
        /// Change product category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpPut]
        [Route("change-category/{ProductId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductCategory([FromQuery] ChangeProductCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}