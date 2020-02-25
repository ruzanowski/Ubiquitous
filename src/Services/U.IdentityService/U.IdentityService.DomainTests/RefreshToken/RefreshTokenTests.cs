using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.DomainTests.User;
using Xunit;

namespace U.IdentityService.DomainTests.RefreshToken
{
    public class RefreshTokenTests
    {
        [Fact]
        public void Should_Create_RefreshToken()
        {
            //arrange
            //act
            var refreshToken = GetRefreshToken();

            //assert
            refreshToken.Id.Should().NotBe(Guid.Empty);
            refreshToken.UserId.Should().NotBe(Guid.Empty);
            refreshToken.Revoked.Should().BeFalse();
            refreshToken.Token.Should().NotBeNullOrEmpty();
            refreshToken.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            refreshToken.RevokedAt.Should().BeNull();
        }

        [Fact]
        public void Should_Revoke_RefreshToken()
        {
            //arrange
            var refreshToken = GetRefreshToken();

            //act
            refreshToken.Revoke();

            //assert
            refreshToken.RevokedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            refreshToken.Revoked.Should().BeTrue();
            refreshToken.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Should_Throw_IdentityException_On_Already_Revoked_RefreshToken()
        {
            //arrange
            var refreshToken = GetRefreshToken();

            //act
            refreshToken.Revoke();
            Action action = () => refreshToken.Revoke();

            //assert
            action.Should().Throw<IdentityException>();
        }

        private Domain.Models.RefreshToken GetRefreshToken()
        {
            var userFixture = new Fixture().Customize(new UserCustomization());
            var user = userFixture.Create<Domain.Models.User>();
            return new Domain.Models.RefreshToken(user, new PasswordHasher<Domain.Models.User>());
        }
    }
}