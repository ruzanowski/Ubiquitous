using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.Common.Jwt;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Events;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.RevokeRefreshToken
{
    public class RevokeRefreshTokenHandler : TokenBaseHandler, IRequestHandler<RevokeRefreshToken>
    {
        public RevokeRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IUserRepository userRepository,
            IClaimsProvider claimsProvider,
            IEventBus busPublisher) : base(refreshTokenRepository,
            jwtService, userRepository, claimsProvider,
            busPublisher)
        {
        }

        public async Task<Unit> Handle(RevokeRefreshToken request, CancellationToken cancellationToken)
        {
            var token = request.Token;
            var userId = request.UserId;

            var refreshToken = await RefreshTokenRepository.GetAsync(token);
            if (refreshToken == null || refreshToken.UserId != userId)
            {
                throw new IdentityException(Codes.RefreshTokenNotFound,
                    "Refresh accessToken was not found.");
            }

            refreshToken.Revoke();
            await RefreshTokenRepository.UpdateAndSaveAsync(refreshToken);
            BusPublisher.Publish(new RefreshTokenRevoked(refreshToken.UserId));

            return Unit.Value;
        }
    }
}