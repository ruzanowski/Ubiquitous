using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using U.IdentityService.Application.Commands.Identity.ChangePassword;
using U.IdentityService.ApplicationTests.Providers;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Commands
{
    public class ChangePasswordTests
    {
        private static (User fakeUser, ChangePasswordHandler sut, IUserRepository userRepository) Arrange()
        {
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = new PasswordHasher<User>();
            var fakeUser = FakeCredentialsProvider.GetUser(passwordHasher);

            var sut = new ChangePasswordHandler(
                userRepository,
                passwordHasher
            );
            return (fakeUser, sut, userRepository);
        }

        [Fact]
        public async Task Should_Change_Password()
        {
            //arrange
            var (fakeUser, sut, userRepository) = Arrange();
            userRepository.GetAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(fakeUser);
            var newPassword = FakeCredentialsProvider.CurrentUserPassword.ToUpper();

            var changePassword = new ChangePassword
            {
                UserId = fakeUser.Id,
                CurrentPassword = FakeCredentialsProvider.CurrentUserPassword,
                NewPassword = newPassword
            };

            //act
            Func<Task> action = async () => await sut.Handle(changePassword, new CancellationToken());

            //assert
            action.Should().NotThrow();
            await Task.CompletedTask;
        }


        [Theory]
        [InlineData("WrongPassword", FakeCredentialsProvider.CurrentUserPassword)]
        [InlineData(FakeCredentialsProvider.CurrentUserPassword, null)]
        [InlineData(null, FakeCredentialsProvider.CurrentUserPassword)]
        [InlineData(null, null)]
        public async Task Should_Throw_IdentityException_On_Incorrect_Credentials(string currentPassword, string newPassword)
        {
            //arrange
            var (fakeUser, sut, userRepository) = Arrange();
            userRepository.GetAsync(Arg.Any<string>()).ReturnsForAnyArgs(fakeUser);
            userRepository.GetAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(fakeUser);

            var changePassword = new ChangePassword
            {
                UserId = fakeUser.Id,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };
            //act
            Func<Task> action = async () => await sut.Handle(changePassword, new CancellationToken());

            //assert
            action.Should().Throw<IdentityException>();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task Should_Throw_IdentityException_On_Not_Existing_User()
        {
            //arrange
            var (fakeUser, sut, _) = Arrange();

            var changePassword = new ChangePassword
            {
                UserId = fakeUser.Id,
                CurrentPassword = FakeCredentialsProvider.CurrentUserPassword,
                NewPassword = "newPassword!"
            };
            //act
            Func<Task> action = async () => await sut.Handle(changePassword, new CancellationToken());

            //assert
            action.Should().Throw<IdentityException>();
            await Task.CompletedTask;
        }
    }
}