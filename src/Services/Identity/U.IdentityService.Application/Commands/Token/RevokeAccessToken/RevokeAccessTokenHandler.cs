using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using U.Common.Jwt;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Services;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.RevokeAccessToken
{
    public class RevokeAccessTokenHandler : TokenBaseHandler, IRequestHandler<RevokeAccessToken>
    {
        private readonly IJwtService _jwtService;

        public RevokeAccessTokenHandler(IOptions<JwtOptions> jwtOptions,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cache,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtHandler jwtHandler,
            IUserRepository userRepository,
            IClaimsProvider claimsProvider,
            IEventBus busPublisher, IJwtService jwtService) : base(jwtOptions,
            httpContextAccessor, cache, refreshTokenRepository,
            jwtHandler, userRepository, claimsProvider,
            busPublisher)
        {
            _jwtService = jwtService;
        }

        public async Task<Unit> Handle(RevokeAccessToken request, CancellationToken cancellationToken)
        {
            await _jwtService.DeactivateCurrentAsync();

            return Unit.Value;
        }
    }
}