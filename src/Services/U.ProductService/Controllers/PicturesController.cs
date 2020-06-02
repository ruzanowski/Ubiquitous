using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Pictures.Commands.AddPicture;
using U.ProductService.Application.Pictures.Commands.DeletePicture;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Pictures.Queries.GetPicture;
using U.ProductService.Application.Pictures.Queries.GetPictures;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// Picture controller of Product service
    /// </summary>
    [Route("api/product/pictures")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PicturesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Picture controller of product service
        /// </summary>
        /// <param name="mediator"></param>
        public PicturesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of command
        /// </summary>
        /// <param name="picturesListQuery"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedItems<PictureViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetPicturesList(
            [FromQuery] GetPicturesListQuery picturesListQuery)
        {
            var queryResult = await _mediator.Send(picturesListQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get picture by its pictureId
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("{pictureId}")]
        [ProducesResponseType(typeof(PaginatedItems<PictureViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPicture([FromRoute] Guid pictureId)
        {
            var queryResult = await _mediator.Send(new GetPictureQuery(pictureId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> AddPicture([FromBody] AddPictureCommand command)
        {
          var picture = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPicture), new {pictureId = picture}, picture);
        }

        /// <summary>
        /// Delete Product Picture
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{pictureId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePicture([FromRoute] Guid pictureId)
        {
            var command = new DeletePictureCommand(pictureId);
            await _mediator.Send(command);
            return Ok();
        }
    }
}