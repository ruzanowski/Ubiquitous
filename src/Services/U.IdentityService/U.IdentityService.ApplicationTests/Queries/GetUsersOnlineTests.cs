using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using U.IdentityService.Application.Queries.GetUsersOnline;
using U.IdentityService.Persistance.Repositories;
using Xunit;

namespace U.IdentityService.ApplicationTests.Queries
{
    public class GetUsersOnlineTests
    {
        private GetUsersOnlineHandler  Arrange()
        {
            var refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            var sut = new GetUsersOnlineHandler(refreshTokenRepository);
            return sut;
        }

        [Fact]
        public async Task Should_Revoke_AccessToken()
        {
            //arrange
            var sut = Arrange();
            var getMyProfile = new GetUsersOnline();

            //act
            var result = await sut.Handle(getMyProfile, new CancellationToken());

            //assert
            result.Should().Be(0);
        }
    }
}