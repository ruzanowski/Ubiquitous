using System.Threading.Tasks;
using U.NotificationService.Application.HttpClients.Identity;

namespace U.NotificationService.Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IIdentityService _identityService;

        public UsersService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var user = await _identityService.GetMyAccountAsync();
            return user;
        }
    }
}