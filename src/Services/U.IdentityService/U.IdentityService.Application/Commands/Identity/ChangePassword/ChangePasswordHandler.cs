using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Identity.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePassword>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ChangePasswordHandler(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var currentPassword = request.CurrentPassword;
            var newPassword = request.NewPassword;

            if (currentPassword is null)
            {
                throw new IdentityException(Codes.ArgumentsCannotBeNull,
                    "Current password cannot be null");
            }

            if (newPassword is null)
            {
                throw new IdentityException(Codes.ArgumentsCannotBeNull,
                    "New password cannot be null");
            }

            var user = await _userRepository.GetAsync(userId);
            if (user is null)
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

            return Unit.Value;
        }
    }
}