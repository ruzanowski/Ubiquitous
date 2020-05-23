using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using U.Common.NetCore.Auth.Models;

namespace U.Common.NetCore.Auth.Service
{
    public interface IJwtService
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
        JsonWebToken CreateToken(string userId, string role = null, IList<Claim> claims = null);
        JsonWebTokenPayload GetTokenPayload(string accessToken);
        JsonWebTokenPayload GetTokenCurrentPayload();
    }
}