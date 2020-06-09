using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace U.ProductService.IntegrationTests.AutoMapper
{
    [Collection("Sequential")]
    public class MapperProfileTests : TestBase
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            using var server = CreateServer();
            var autoMapper = server.Host.Services.GetService<IMapper>();
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}