using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Commands.Identity.SignUp;
using U.IdentityService.ApplicationTests.Helpers;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Commands.SignUp
{
    public class SignUpTests
    {
        private static (User fakeUser, SignUpHandler sut, IUserRepository userRepository) Arrange()
        {
            var eventBus = Substitute.For<IEventBus>();
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = new PasswordHasher<User>();
            var fakeUser = FakeCredentialsProvider.GetUser(passwordHasher);

            var sut = new SignUpHandler(
                userRepository,
                passwordHasher,
                eventBus
            );
            return (fakeUser, sut, userRepository);
        }


        [Fact]
        public async Task Should_SignUp()
        {
            //arrange
            var (fakeUser, sut, _) = Arrange();

            //act
            Func<Task> result = async () => await sut.Handle(new Application.Commands.Identity.SignUp.SignUp
            {
                Email = fakeUser.Email,
                Password = FakeCredentialsProvider.UserPassword,
                Nickname = fakeUser.Nickname,
                Role = fakeUser.Role
            }, new CancellationToken());

            //assert
            result.Should().NotThrow();
            await Task.CompletedTask;

        }

        [Fact]
        public async Task Should_Throw_On_Already_Existing_User()
        {
            //arrange
            var (fakeUser, sut, userRepository) = Arrange();
            userRepository.GetAsync(Arg.Any<string>()).ReturnsForAnyArgs(fakeUser);

            var signUp = new Application.Commands.Identity.SignUp.SignUp
            {
                Email = fakeUser.Email,
                Password = FakeCredentialsProvider.UserPassword,
                Nickname = fakeUser.Nickname,
                Role = fakeUser.Role
            };

            //act
            Func<Task> result = async () => await sut.Handle(signUp, new CancellationToken());

            //assert
            result.Should().Throw<IdentityException>();
            await Task.CompletedTask;
        }
    }
}