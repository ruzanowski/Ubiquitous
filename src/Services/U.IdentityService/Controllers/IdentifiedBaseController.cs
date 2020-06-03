using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace U.IdentityService.Controllers
{
    [ExcludeFromCodeCoverage]
    public class IdentifiedBaseController : ControllerBase
    {
        protected Guid UserId
            => string.IsNullOrWhiteSpace(User?.Identity?.Name) ?
                Guid.Empty :
                Guid.Parse(User.Identity.Name);
    }
}