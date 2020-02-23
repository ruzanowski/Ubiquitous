using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace U.IdentityService.DomainTests.User
{
    public class UserTests
    {
        [Fact]
        public void Should_Create_User()
        {
            //arrange
            var fixture = new Fixture().Customize(new UserCustomization());

            //act
            var user = fixture.Create<Domain.Models.User>();

            //assert
            user.Id.Should().NotBeEmpty();
            user.Nickname.Should().NotBeEmpty();
            user.Email.Should().NotBeEmpty();
            user.Email.Should().Contain("@");
            user.Role.Should().NotBeEmpty();
            user.PasswordHash.Should().BeNullOrEmpty();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Should_Set_Password()
        {
            //arrange
            var fixture = new Fixture().Customize(new UserCustomization());
            var user = fixture.Create<Domain.Models.User>();
            var password = new Fixture().Create<string>();
            var passwordHasher = new PasswordHasher<Domain.Models.User>();

            //act
            user.SetPassword(password, passwordHasher);
            var validationResult = user.ValidatePassword(password, passwordHasher);

            //assert
            user.PasswordHash.Should().NotBeNullOrEmpty();
            user.PasswordHash.Should().NotBe(password);
            validationResult.Should().BeTrue();
        }

    }
}