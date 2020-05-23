using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.Common.NetCore.Auth.Service;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Services;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.RevokeAccessToken
{
    public class RevokeAccessTokenHandler : TokenBaseHandler, IRequestHandler<RevokeAccessToken>
    {
        public RevokeAccessTokenHandler(IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IUserRepository userRepository,
            IClaimsProvider claimsProvider,
            IEventBus busPublisher) : base(refreshTokenRepository, jwtService,
            userRepository, claimsProvider,
            busPublisher)
        {
        }

        public async Task<Unit> Handle(RevokeAccessToken request, CancellationToken cancellationToken)
        {
            await JwtService.DeactivateCurrentAsync();

            return Unit.Value;
        }
    }
}