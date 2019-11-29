using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using U.EventBus.Abstractions;
using U.EventBus.Events.Identity;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Identity.SignUp
{
    public class SingUpHandler : IRequestHandler<SignUp>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEventBus _busPublisher;

        public SingUpHandler(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IEventBus busPublisher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _busPublisher = busPublisher;
        }

        public async Task<Unit> Handle(SignUp request, CancellationToken cancellationToken)
        {
            var role = request.Role;
            var id = request.Id;
            var email = request.Email;
            var nickname = request.Nickname;
            var password = request.Password;

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

            user = new User(id, email, nickname, role);
            user.SetPassword(password, _passwordHasher);
            await _userRepository.AddAndSaveAsync(user);

            _busPublisher.Publish(new SignedUpIntegrationEvent(id, email, role));

            return Unit.Value;
        }
    }
}