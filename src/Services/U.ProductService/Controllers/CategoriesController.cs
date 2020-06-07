using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Categories.Commands.Create;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Categories.Queries.GetCategories;
using U.ProductService.Application.Categories.Queries.GetCategoriesCount;
using U.ProductService.Application.Categories.Queries.GetCategory;

namespace U.ProductService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/product/categories")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mediator"></param>
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of categories
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedItems<CategoryViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetList([FromQuery] GetCategoriesListQuery query)
        {
            var queryResult = await _mediator.Send(query);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get categories by categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{categoryId}")]
        [ProducesResponseType(typeof(PaginatedItems<CategoryViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid categoryId)
        {
            var queryResult = await _mediator.Send(new GetCategoryQuery(categoryId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var category = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {categoryId = category.Id}, category);
        }

        /// <summary>
        /// Get categories total count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCount([FromQuery] GetCategoriesCount query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
    }
}