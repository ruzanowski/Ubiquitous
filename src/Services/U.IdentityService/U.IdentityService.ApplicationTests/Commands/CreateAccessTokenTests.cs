using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using U.Common.NetCore.Auth.Models;
using U.Common.NetCore.Auth.Service;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Commands.Token.CreateAccessToken;
using U.IdentityService.Application.Services;
using U.IdentityService.ApplicationTests.Providers;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Commands
{
    public class CreateAccessTokenTests
    {
        private static (User fakeUser, CreateAccessTokenHandler sut, IUserRepository userRepository) Arrange()
        {
            var claimsProvider = Substitute.For<IClaimsProvider>();
            var refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            var jwtService = Substitute.For<IJwtService>();
            var userRepository = Substitute.For<IUserRepository>();
            var eventBus = Substitute.For<IEventBus>();

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

            var sut = new CreateAccessTokenHandler(
                refreshTokenRepository,
                jwtService,
                userRepository,
                claimsProvider,
                eventBus,
                passwordHasher
            );
            return (fakeUser, sut, userRepository);
        }

        [Fact]
        public async Task Should_Create_AccessToken()
        {
            //arrange
            var (fakeUser, sut, userRepository) = Arrange();
            userRepository.GetAsync(Arg.Any<string>()).ReturnsForAnyArgs(fakeUser);
            userRepository.GetAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(fakeUser);
            var createAccessToken = new CreateAccessToken(fakeUser.Id);

            //act
            Func<Task> action = async () => await sut.Handle(createAccessToken, new CancellationToken());

            //assert
            action.Should().NotThrow();
            await Task.CompletedTask;
        }


        [Fact]
        public async Task Should_Throw_IdentityException_On_Not_Existing_User()
        {
            //arrange
            var (fakeUser, sut, _) = Arrange();
            var createAccessToken = new CreateAccessToken(fakeUser.Id);

            //act
            Func<Task> action = async () => await sut.Handle(createAccessToken, new CancellationToken());

            //assert
            action.Should().Throw<IdentityException>();
            await Task.CompletedTask;
        }
    }
}