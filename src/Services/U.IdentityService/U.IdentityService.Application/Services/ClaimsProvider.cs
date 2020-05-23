using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using U.Common.NetCore.Auth.Claims;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Services
{
    public class ClaimsProvider : IClaimsProvider
    {
        private readonly IUserRepository _userRepository;

        public ClaimsProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IList<Claim>> GetAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);

            var jwtClaims = new List<Claim>
            {
                new Claim(JwtClaimsTypes.Email, user.Email),
                new Claim(JwtClaimsTypes.Nickname, user.Nickname),
                new Claim(JwtClaimsTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimsTypes.Role, user.Role)
            };

            return jwtClaims;
        }
    }
}