using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Manufacturers.Commands.AddPicture;
using U.ProductService.Application.Manufacturers.Commands.Create;
using U.ProductService.Application.Manufacturers.Commands.DeletePicture;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Application.Manufacturers.Queries.GetCount;
using U.ProductService.Application.Manufacturers.Queries.GetList;
using U.ProductService.Application.Manufacturers.Queries.GetSingle;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// Manufacturer controller of Product service
    /// </summary>
    [Route("api/product/manufacturers")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Manufacturer controller of Product service
        /// </summary>
        /// <param name="mediator"></param>
        public ManufacturersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of command
        /// </summary>
        /// <param name="manufacturersListQuery"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedItems<ManufacturerViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetManufacturersList(
            [FromQuery] GetManufacturersListQuery manufacturersListQuery)
        {
            var queryResult = await _mediator.Send(manufacturersListQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get manufacturer by its ManufacturerId
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("{manufacturerId}")]
        [ProducesResponseType(typeof(PaginatedItems<ManufacturerViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetManufacturer([FromRoute] Guid manufacturerId)
        {
            var queryResult = await _mediator.Send(new QueryManufacturer(manufacturerId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Create manufacturer
        /// </summary>
        /// <param name="manufacturers"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateManufacturer([FromBody] CreateManufacturerCommand manufacturers)
        {
            var manufacturerId = await _mediator.Send(manufacturers);
            return CreatedAtAction(nameof(CreateManufacturer), manufacturerId);
        }

        /// <summary>
        /// Add manufacturer picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{manufacturerId}/picture")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddPicture([FromQuery] AddManufacturerPictureCommand command)
        {
            var pictureId = await _mediator.Send(command);
            return Ok(pictureId);
        }

        /// <summary>
        /// Add manufacturer picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete]
        [Route("{manufacturerId}/picture/{pictureId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePicture([FromRoute] DeleteManufacturerPictureCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get manufacturers total count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCount([FromQuery] GetManufacturersCount query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
    }
}