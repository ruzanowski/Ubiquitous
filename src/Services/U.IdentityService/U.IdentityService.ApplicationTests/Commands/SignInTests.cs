using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using U.Common.NetCore.Auth.Models;
using U.Common.NetCore.Auth.Service;
using U.IdentityService.Application.Commands.Identity.SignIn;
using U.IdentityService.Application.Services;
using U.IdentityService.ApplicationTests.Providers;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Commands
{
    public class SignInTests
    {
        private (User fakeUser, SignInHandler sut, IUserRepository userRepository, JsonWebToken fakeJwt) Arrange()
        {
            var claimsProvider = Substitute.For<IClaimsProvider>();
            var refreshTokenRepository= Substitute.For<IRefreshTokenRepository>();
            var jwtService = Substitute.For<IJwtService>();
            var userRepository = Substitute.For<IUserRepository>();

            var fakeJwt = new JsonWebToken
            {
                Id = Guid.NewGuid().ToString(),
                Claims = new Dictionary<string, string>
                {
                    {
                        "first_claim", "first_claim_value"
                    },
                    {
                        "second_claim", "second_claim_value"
                    }
                },
                Expires = DateTime.UtcNow.AddMinutes(1),
                Role = Role.User,
                AccessToken = Guid.NewGuid().ToString(),
                RefreshToken = Guid.NewGuid().ToString()
            };
            jwtService.CreateToken(Arg.Any<string>()).ReturnsForAnyArgs(fakeJwt);

            var passwordHasher = new PasswordHasher<User>();
            var fakeUser = FakeCredentialsProvider.GetUser(passwordHasher);

            var sut = new SignInHandler(
                claimsProvider,
                refreshTokenRepository,
                jwtService,
                passwordHasher,
                userRepository
            );
            return (fakeUser, sut, userRepository, fakeJwt);
        }

        [Fact]
        public async Task Should_SignIn()
        {
            //arrange
            var (fakeUser, sut, userRepository, fakeJwt) = Arrange();
            userRepository.GetAsync(Arg.Any<string>()).ReturnsForAnyArgs(fakeUser);

            //act
            var result = await sut.Handle(new SignIn
            {
                Email = fakeUser.Email,
                Password = FakeCredentialsProvider.CurrentUserPassword
            }, new CancellationToken());

            //assert
            result.Id.Should().NotBeEmpty();
            result.Claims.Should().NotBeEmpty();
            result.Expires.Should().BeAfter(DateTime.UtcNow);
            Role.IsValid(result.Role).Should().BeTrue();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.Should().Be(fakeJwt);
        }

        [Fact]
        public async Task Should_Throw_On_Incorrect_Credentials()
        {
            //arrange
            var (fakeUser, sut, _, _) = Arrange();

            //act
            Func<Task> result = async () => await sut.Handle(new SignIn
            {
                Email = fakeUser.Email,
                Password = FakeCredentialsProvider.CurrentUserPassword
            }, new CancellationToken());

            //assert
            result.Should().Throw<IdentityException>();
            await Task.CompletedTask;
        }
    }
}