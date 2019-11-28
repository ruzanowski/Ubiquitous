using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.Common.Jwt;
using U.EventBus.Abstractions;
using U.EventBus.Events.Identity;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.RefreshAccessToken
{
    public class RefreshAccessTokenHandler : TokenBaseHandler,
        IRequestHandler<RefreshAccessToken, JsonWebToken>
    {
        public RefreshAccessTokenHandler(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService,
            IUserRepository userRepository, IClaimsProvider claimsProvider, IEventBus busPublisher) : base(
            refreshTokenRepository, jwtService, userRepository, claimsProvider,
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
                    "Refresh accessToken was not found.");
            }

            if (refreshToken.Revoked)
            {
                throw new IdentityException(Codes.RefreshTokenAlreadyRevoked,
                    $"Refresh accessToken: '{refreshToken.Id}' was revoked.");
            }

            var user = await GetUserOrThrowAsync(refreshToken.UserId);

            var claims = await ClaimsProvider.GetAsync(user.Id);
            var jwt = JwtService.CreateToken(user.Id.ToString("N"), user.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            BusPublisher.Publish(new AccessTokenRefreshed(user.Id));

            return jwt;
        }
    }
}