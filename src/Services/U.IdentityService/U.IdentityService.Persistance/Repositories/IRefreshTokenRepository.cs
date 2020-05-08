using System.Collections.Generic;
using System.Threading.Tasks;
using U.IdentityService.Domain.Models;

namespace U.IdentityService.Persistance.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAndSaveAsync(RefreshToken token);
        Task UpdateAndSaveAsync(RefreshToken token);
        Task<List<RefreshToken>> GetActiveAsync();
    }
}