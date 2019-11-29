using System.Threading.Tasks;
using U.NotificationService.Application.HttpClients.Identity;

namespace U.NotificationService.Application.Services.Users
{
    public interface IUsersService
    {
        Task<UserDto> GetCurrentUserAsync();
    }
}