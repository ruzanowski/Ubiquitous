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
    ///
    /// </summary>
    [Route("api/product/pictures")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PicturesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        ///
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
        /// Add Picture
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
        /// Delete Picture
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

        /// <summary>
        /// Update picture's properties
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{pictureId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdatePicture([FromRoute] Guid pictureId, [FromBody] UpdatePictureCommand command)
        {
            command.PictureId = pictureId;
            await _mediator.Send(command);
            return Ok();
        }
    }
}