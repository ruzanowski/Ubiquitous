using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.SmartStoreAdapter.Application.Models.Pictures;

namespace U.SmartStoreAdapter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/smartstore/pictures")]
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("store")]
        public async Task<IActionResult> StorePicture(StorePicturesCommand storePictureCommand)
        {
            var result = await _mediator.Send(storePictureCommand);
            return Ok(result);
        }
    }
}