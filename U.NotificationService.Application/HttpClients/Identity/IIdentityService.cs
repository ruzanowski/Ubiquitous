using System.Threading.Tasks;
using RestEase;
using U.Common.Jwt;

namespace U.NotificationService.Application.HttpClients.Identity
{
    public interface IIdentityService
    {
        [AllowAnyStatusCode]
        [Get("api/identity/users/me")]
        [JwtAuth]
        Task<UserDto> GetMyAccountAsync();
    }
}