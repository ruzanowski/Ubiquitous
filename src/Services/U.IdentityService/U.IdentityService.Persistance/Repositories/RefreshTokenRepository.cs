using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.Jwt.Models;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Contexts;

namespace U.IdentityService.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityContext _identityContext;
        private readonly JwtOptions _jwtOptions;

        public RefreshTokenRepository(IdentityContext identityContext, JwtOptions jwtOptions)
        {
            _identityContext = identityContext;
            _jwtOptions = jwtOptions;
        }

        public async Task<RefreshToken> GetAsync(string token)
            => await _identityContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

        public async Task<List<RefreshToken>> GetActiveAsync()
        {
            var activeTokens =  await _identityContext.RefreshTokens
                .Where(x => !x.Revoked &&
                            DateTime.UtcNow <= x.CreatedAt.AddMinutes(_jwtOptions.ExpiryMinutes))
                .ToListAsync();

            var uniqueUserTokens = new List<Guid>();
            var tokens = new List<RefreshToken>();

            foreach (var refreshToken in activeTokens)
            {
                if (uniqueUserTokens.Contains(refreshToken.UserId)) continue;

                uniqueUserTokens.Add(refreshToken.UserId);
                tokens.Add(refreshToken);
            }

            return tokens;
        }

        public async Task AddAndSaveAsync(RefreshToken token)
        {
            await _identityContext.RefreshTokens.AddAsync(token);
            await _identityContext.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(RefreshToken token)
        {
            _identityContext.RefreshTokens.Update(token);
            await _identityContext.SaveChangesAsync();

        }
    }
}