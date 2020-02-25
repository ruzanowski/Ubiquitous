using AutoFixture;
using Microsoft.AspNetCore.Identity;
using U.IdentityService.Domain.Models;
using U.IdentityService.DomainTests.User;

namespace U.IdentityService.ApplicationTests.Providers
{
    public static class FakeCredentialsProvider
    {
        internal const string CurrentUserPassword = "Password1!";

        internal static User GetUser(IPasswordHasher<User> passwordHasher)
        {
            var fixture = new Fixture().Customize(new UserCustomization());

            var user = fixture.Create<User>();
            user.SetPassword(CurrentUserPassword, passwordHasher);

            return user;
        }
    }
}