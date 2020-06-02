using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.AttachPicture;
using U.ProductService.Application.Products.Commands.ChangeCategory;
using U.ProductService.Application.Products.Commands.ChangePrice;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Publish;
using U.ProductService.Application.Products.Commands.UnPublish;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.GetCount;
using U.ProductService.Application.Products.Queries.GetList;
using U.ProductService.Application.Products.Queries.GetSingle;
using U.ProductService.Application.Products.Queries.GetSingleByExternalTuple;
using U.ProductService.Application.Products.Queries.GetStatistics;
using U.ProductService.Application.Products.Queries.GetStatisticsByCategory;
using U.ProductService.Application.Products.Queries.GetStatisticsByManufacturers;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// Product controller of product service
    /// </summary>
    [Route("api/product/products")]
    [ApiController]
    [ExcludeFromCodeCoverage]
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
        /// Get list of query
        /// </summary>
        /// <param name="productsListQuery"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("")]
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
        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId)
        {
            var queryResult = await _mediator.Send(new QueryProduct(productId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by external tuple
        /// </summary>
        /// <param name="externalSourceName"></param>
        /// <param name="externalSourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("externalTuple")]
        [ProducesResponseType(typeof(ProductViewModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductByExternalTuple(string externalSourceName, string externalSourceId)
        {
            var queryResult = await _mediator.Send(new QueryProductByExternalTuple
            {
                ExternalSourceName = externalSourceName,
                ExternalSourceId = externalSourceId
            });
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
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand products)
        {
            var product = await _mediator.Send(products);
            return CreatedAtAction(nameof(GetProduct), new {productId = product.Id}, product);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{productId}")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductCommand command)
        {
            command.Id = productId;
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Publish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/publish")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> PublishProduct([FromRoute] Guid productId)
        {
            await _mediator.Send(new PublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// Unpublish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/unpublish")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UnPublishProduct([FromRoute] Guid productId)
        {
            await _mediator.Send(new UnPublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// Change price of the product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{productId}/price")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductPrice([FromRoute] Guid productId, [FromQuery] decimal price)
        {
            await _mediator.Send(new ChangeProductPriceCommand(productId, price));
            return Ok();
        }

        /// <summary>
        /// Attach picture to product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{productId}/picture/{pictureId}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> AttachPicture([FromRoute] Guid productId, [FromRoute] Guid pictureId)
        {
            await _mediator.Send(new AttachPictureToProductCommand(productId, pictureId));
            return Ok();
        }

        /// <summary>
        /// Detach picture out of product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{productId}/picture/{pictureId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DetachPicture([FromRoute] Guid productId, [FromRoute] Guid pictureId)
        {
            var command = new DetachPictureToProductCommand(productId, pictureId);
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get Product statistics
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/creation")]
        [ProducesResponseType(typeof(ProductStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductCreationStatistics([FromQuery] GetProductsStatisticsQuery query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
        /// <summary>
        /// Get Product statistics by manufacturer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/manufacturer")]
        [ProducesResponseType(typeof(ProductByManufacturersStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductStatisticsByManufacturer([FromQuery] GetProductsStatisticsByManufacturers query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
        /// <summary>
        /// Get Product statistics by productCategory
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/category")]
        [ProducesResponseType(typeof(ProductByCategoryStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductStatisticsByCategory([FromQuery] GetProductsStatisticsByCategory query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }

        /// <summary>
        /// Change category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{productId}/category")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductCategory([FromQuery] ChangeProductCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        /// <summary>
        /// Get Product total count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCount([FromQuery] GetProductsCount query)
        {
            var count = await _mediator.Send(query);
            return Ok(count);
        }
    }
}