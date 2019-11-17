using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using U.Common.Authentication;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Events;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IEventBus _busPublisher;

        public IdentityService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtHandler jwtHandler,
            IRefreshTokenRepository refreshTokenRepository,
            IClaimsProvider claimsProvider,
            IEventBus busPublisher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
            _refreshTokenRepository = refreshTokenRepository;
            _claimsProvider = claimsProvider;
            _busPublisher = busPublisher;
        }

        public async Task SignUpAsync(Guid id, string email, string password, string role = Role.User)
        {
            if (email is null)
            {
                throw new ArgumentException();
            }

            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new IdentityException(Codes.EmailInUse,
                    $"Email: '{email}' is already in use.");
            }
            if (string.IsNullOrWhiteSpace(role))
            {
                 role = Role.User;
            }
            user = new User(id, email, role);
            user.SetPassword(password, _passwordHasher);
            await _userRepository.AddAndSaveAsync(user);

            _busPublisher.Publish(new SignedUp(id, email, role));
        }

        public async Task<JsonWebToken> SignInAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null || !user.ValidatePassword(password, _passwordHasher))
            {
                throw new IdentityException(Codes.InvalidCredentials,
                    "Invalid credentials.");
            }
            var refreshToken = new RefreshToken(user, _passwordHasher);
            var claims = await _claimsProvider.GetAsync(user.Id);
            var jwt = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            await _refreshTokenRepository.AddAndSaveAsync(refreshToken);

            return jwt;
        }

        public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new IdentityException(Codes.UserNotFound,
                    $"User with id: '{userId}' was not found.");
            }
            if (!user.ValidatePassword(currentPassword, _passwordHasher))
            {
                throw new IdentityException(Codes.InvalidCurrentPassword,
                    "Invalid current password.");
            }
            user.SetPassword(newPassword, _passwordHasher);
            await _userRepository.UpdateAndSaveAsync(user);
            _busPublisher.Publish(new PasswordChanged(userId));
        }
    }
}