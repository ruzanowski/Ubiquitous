using System;
using System.Threading.Tasks;
using JsonWebToken = U.Common.Authentication.JsonWebToken;

namespace U.IdentityService.Application.Services
{
    public interface IRefreshTokenService
    {
        Task AddAsync(Guid userId);
        Task<JsonWebToken> CreateAccessTokenAsync(string refreshToken);
        Task RevokeAsync(string refreshToken, Guid userId);
    }
}