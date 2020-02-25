using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using U.Common.Jwt.Models;
using U.Common.Jwt.Service;
using U.IdentityService.Application.Queries.GetMyAccount;
using U.IdentityService.ApplicationTests.Providers;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Queries
{
    public class GetMyProfileTests
    {
        private (User fakeUser, GetMyProfileHandler sut, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)  Arrange()
        {
            var jwtService = Substitute.For<IJwtService>();
            var userRepository = Substitute.For<IUserRepository>();
            var refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();

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

            var sut = new GetMyProfileHandler(userRepository);
            return (fakeUser, sut, userRepository, refreshTokenRepository);
        }

        [Fact]
        public async Task Should_Revoke_AccessToken()
        {
            //arrange
            var (fakeUser, sut, userRepository, _) = Arrange();
            userRepository.GetAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(fakeUser);
            var getMyProfile = new GetMyProfile(fakeUser.Id);

            //act
            var result = await sut.Handle(getMyProfile, new CancellationToken());

            //assert
            result.Id.Should().NotBeEmpty();
            result.Id.Should().Be(fakeUser.Id);
            result.Nickname.Should().NotBeEmpty();
            result.Nickname.Should().Be(fakeUser.Nickname);
            result.Email.Should().NotBeEmpty();
            result.Email.Should().Be(fakeUser.Email);
            result.Email.Should().Contain("@");
            result.Email.Should().Be(fakeUser.Email);
            result.Role.Should().NotBeEmpty();
            result.Role.Should().Be(fakeUser.Role);
            result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            result.CreatedAt.Should().Be(fakeUser.CreatedAt);
            result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            result.UpdatedAt.Should().Be(fakeUser.UpdatedAt);
        }
    }
}