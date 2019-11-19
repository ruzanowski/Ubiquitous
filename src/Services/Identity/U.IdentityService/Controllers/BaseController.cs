using System;
using Microsoft.AspNetCore.Mvc;

namespace U.IdentityService.Controllers
{
    public class IdentifiedBaseController : ControllerBase
    {
        protected bool IsAdmin
            => User.IsInRole("admin");

        protected Guid UserId
            => string.IsNullOrWhiteSpace(User?.Identity?.Name) ? 
                Guid.Empty : 
                Guid.Parse(User.Identity.Name);
    }
}