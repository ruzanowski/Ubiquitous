using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using U.IdentityService.Domain.Domain;
using Xunit;

namespace U.IdentityService.DomainTests
{
    public class UserTests
    {
        [Fact]
        public void Should_Test_Pass()
        {
            true.Should().BeTrue();
        }

        [Theory]
        [UserTestData]
        public async Task Should_Create_User(User user)
        {
            user.Id.Should().NotBeEmpty();
            user.Nickname.Should().NotBeEmpty();
            user.Email.Should().NotBeEmpty();
            user.Email.Should().Contain("@");
            user.Role.Should().NotBeEmpty();
            user.PasswordHash.Should().BeNullOrEmpty();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Theory, UserTestData]
        public void Should_Set_Password(User user)
        {
            //arrange
            var password = new Fixture().Create<string>();
            var passwordHasher = new PasswordHasher<User>();

            //act
            user.SetPassword(password, passwordHasher);
            var validationResult = user.ValidatePassword(password, passwordHasher);

            //assert
            user.PasswordHash.Should().NotBeNullOrEmpty();
            validationResult.Should().BeTrue();
        }

    }
}