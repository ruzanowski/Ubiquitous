using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U.Common.NetCore.Consul;

namespace U.IdentityService.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/identity")]
    [ExcludeFromCodeCoverage]
    public class HealthController : ControllerBase
    {
        private readonly IConsulIdentifierService _identifierService;

        public HealthController(IConsulIdentifierService identifierService)
        {
            _identifierService = identifierService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet("health/{serviceId}")]
        [AllowAnonymous]
        public IActionResult HealthCheck(string serviceId)
        {
            return IsCorrectServiceId(serviceId) ? (IActionResult) NoContent() : BadRequest();
        }

        private bool IsCorrectServiceId(string guid) => _identifierService.IsTheSame(guid);
    }
}