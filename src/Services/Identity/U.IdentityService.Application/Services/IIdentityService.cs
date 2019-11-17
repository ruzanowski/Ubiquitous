using System;
using System.Threading.Tasks;
using U.IdentityService.Domain.Domain;
using JsonWebToken = U.Common.Authentication.JsonWebToken;

namespace U.IdentityService.Application.Services
{
    public interface IIdentityService
    {
        Task SignUpAsync(Guid id, string email, string password, string role = Role.User);
        Task<JsonWebToken> SignInAsync(string email, string password);
        Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    }
}