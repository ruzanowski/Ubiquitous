using AutoFixture;
using AutoFixture.Xunit2;

namespace U.IdentityService.DomainTests
{
    public class UserTestDataAttribute : AutoDataAttribute
    {
        public UserTestDataAttribute() : base(() =>
            new Fixture().Customize(new CompositeCustomization(new UserCustomization())))
        {
        }
    }
}