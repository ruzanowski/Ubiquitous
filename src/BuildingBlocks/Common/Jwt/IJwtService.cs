using System.Threading.Tasks;

namespace U.Common.Jwt
{
    public interface IJwtService
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }
}