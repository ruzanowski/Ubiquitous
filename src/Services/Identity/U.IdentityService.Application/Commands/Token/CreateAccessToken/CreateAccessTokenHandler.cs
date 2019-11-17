using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using U.Common.Jwt;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.CreateAccessToken
{
    public class CreateAccessTokenHandler : TokenBaseHandler, IRequestHandler<CreateAccessToken>
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateAccessTokenHandler(IOptions<JwtOptions> jwtOptions,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cache,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtHandler jwtHandler,
            IUserRepository userRepository,
            IClaimsProvider claimsProvider,
            IEventBus busPublisher, IPasswordHasher<User> passwordHasher) : base(jwtOptions,
            httpContextAccessor, cache, refreshTokenRepository,
            jwtHandler, userRepository, claimsProvider,
            busPublisher)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(CreateAccessToken request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await UserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new IdentityException(Codes.UserNotFound,
                    $"User: '{userId}' was not found.");
            }

            await RefreshTokenRepository.AddAndSaveAsync(new RefreshToken(user, _passwordHasher));

            return Unit.Value;
        }
    }
}