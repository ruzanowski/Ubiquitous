using System;
using AutoFixture;
using U.IdentityService.Domain.Domain;

namespace U.IdentityService.DomainTests
{
    public class UserCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var id = Guid.NewGuid();
                var emailPrefix = fixture.Create<string>();
                var userEmail = $"{emailPrefix}@ubiquitous.com";
                var nickname = fixture.Create<string>();
                var role = Role.User;
                return new User(id, userEmail, nickname, role);
            });
        }
    }
}