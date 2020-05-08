using System;
using System.Threading.Tasks;
using U.Common.Jwt;
using U.Common.Jwt.Service;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Token
{
    public class TokenBaseHandler
    {
        protected readonly IRefreshTokenRepository RefreshTokenRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly IJwtService JwtService;
        protected readonly IClaimsProvider ClaimsProvider;
        protected readonly IEventBus BusPublisher;

        public TokenBaseHandler(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService, IUserRepository userRepository, IClaimsProvider claimsProvider, IEventBus busPublisher)
        {
            RefreshTokenRepository = refreshTokenRepository;
            JwtService = jwtService;
            UserRepository = userRepository;
            ClaimsProvider = claimsProvider;
            BusPublisher = busPublisher;
        }

        protected async Task<User> GetUserOrThrowAsync(Guid userId)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new IdentityException(Codes.UserNotFound,
                    $"User: '{userId}' was not found.");
            }

            return user;
        }
    }
}