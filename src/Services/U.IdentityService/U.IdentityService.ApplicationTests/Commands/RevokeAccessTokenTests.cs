using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using U.Common.Jwt.Models;
using U.Common.Jwt.Service;
using U.EventBus.Abstractions;
using U.IdentityService.Application.Commands.Token.RevokeAccessToken;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Commands
{
    public class RevokeAccessTokenTests
    {
        private RevokeAccessTokenHandler Arrange()
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

            var sut = new RevokeAccessTokenHandler(
                refreshTokenRepository,
                jwtService,
                userRepository,
                claimsProvider,
                eventBus
            );
            return sut;
        }

        [Fact]
        public async Task Should_Revoke_AccessToken()
        {
            //arrange
            var sut = Arrange();

            var createAccessToken = new RevokeAccessToken();

            //act
            Func<Task> action = async () => await sut.Handle(createAccessToken, new CancellationToken());

            //assert
            action.Should().NotThrow();
            await Task.CompletedTask;
        }
    }
}