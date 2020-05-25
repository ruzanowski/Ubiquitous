using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using U.Common.NetCore.Auth.Service;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token.CreateAccessToken
{
    public class CreateAccessTokenHandler : TokenBaseHandler, IRequestHandler<CreateAccessToken>
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateAccessTokenHandler(IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IUserRepository userRepository,
            IClaimsProvider claimsProvider,
            IEventBus busPublisher, IPasswordHasher<User> passwordHasher) : base(refreshTokenRepository,
            jwtService, userRepository, claimsProvider,
            busPublisher)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(CreateAccessToken request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await GetUserOrThrowAsync(userId);

            await RefreshTokenRepository.AddAndSaveAsync(new RefreshToken(user, _passwordHasher));

            return Unit.Value;
        }
    }
}