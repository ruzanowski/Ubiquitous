using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Persistance.Contexts;

namespace U.IdentityService.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityContext _identityContext;

        public RefreshTokenRepository(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<RefreshToken> GetAsync(string token)
            => await _identityContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

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