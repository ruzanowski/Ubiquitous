using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Pictures.Queries.GetList;
using U.ProductService.Application.Pictures.Queries.GetSingle;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// Picture controller of Product service
    /// </summary>
    [Route("api/product/pictures")]
    [ApiController]
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
    }
}