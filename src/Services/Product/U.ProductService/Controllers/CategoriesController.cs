using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Jwt;
using U.Common.Pagination;
using U.ProductService.Application.Categories.Commands.Create;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Categories.Queries.GetList;
using U.ProductService.Application.Categories.Queries.GetSingle;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// category controller of Product service
    /// </summary>
    [Route("api/product/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// category controller of Product service
        /// </summary>
        /// <param name="mediator"></param>
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of command
        /// </summary>
        /// <param name="categorysListQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query")]
        [ProducesResponseType(typeof(PaginatedItems<CategoryViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategoriesList(
            [FromQuery] GetCategoriesListQuery categorysListQuery)
        {
            var queryResult = await _mediator.Send(categorysListQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get category by its categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query/{categoryId:Guid}")]
        [ProducesResponseType(typeof(PaginatedItems<CategoryViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategory([FromRoute] Guid categoryId)
        {
            var queryResult = await _mediator.Send(new GetCategoryQuery(categoryId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Create Category
        /// </summary>
        /// <param name="manufacturers"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateManufacturer([FromBody] CreateCategoryCommand categories)
        {
            var categoryId = await _mediator.Send(categories);
            return CreatedAtAction(nameof(CreateManufacturer), categoryId);
        }
    }
}