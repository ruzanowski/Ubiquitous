using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using U.Common.Jwt;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Events;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.RefreshAccessToken
{
    public class RefreshAccessTokenHandler : TokenBaseHandler,
        IRequestHandler<RefreshAccessToken, JsonWebToken>
    {
        public RefreshAccessTokenHandler(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor,
            IDistributedCache cache, IRefreshTokenRepository refreshTokenRepository, IJwtHandler jwtHandler,
            IUserRepository userRepository, IClaimsProvider claimsProvider, IEventBus busPublisher) : base(jwtOptions,
            httpContextAccessor, cache, refreshTokenRepository, jwtHandler, userRepository, claimsProvider,
            busPublisher)
        {
        }

        public async Task<JsonWebToken> Handle(RefreshAccessToken request,
            CancellationToken cancellationToken)
        {
            var token = request.Token;

            var refreshToken = await RefreshTokenRepository.GetAsync(token);
            if (refreshToken == null)
            {
                throw new IdentityException(Codes.RefreshTokenNotFound,
                    "Refresh token was not found.");
            }

            if (refreshToken.Revoked)
            {
                throw new IdentityException(Codes.RefreshTokenAlreadyRevoked,
                    $"Refresh token: '{refreshToken.Id}' was revoked.");
            }

            var user = await UserRepository.GetAsync(refreshToken.UserId);
            if (user == null)
            {
                throw new IdentityException(Codes.UserNotFound,
                    $"User: '{refreshToken.UserId}' was not found.");
            }

            var claims = await ClaimsProvider.GetAsync(user.Id);
            var jwt = JwtHandler.CreateToken(user.Id.ToString("N"), user.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            BusPublisher.Publish(new AccessTokenRefreshed(user.Id));

            return jwt;
        }
    }
}