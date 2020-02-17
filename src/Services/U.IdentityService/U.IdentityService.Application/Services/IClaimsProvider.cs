using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace U.IdentityService.Application.Services
{
    public interface IClaimsProvider
    {
         Task<IList<Claim>> GetAsync(Guid userId);
    }
}