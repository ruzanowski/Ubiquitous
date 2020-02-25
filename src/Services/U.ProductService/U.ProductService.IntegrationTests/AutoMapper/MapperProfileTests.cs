using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace U.ProductService.IntegrationTests.AutoMapper
{
    public class MapperProfileTests : TestBase
    {
        [Fact(Skip = "v.0.3")]
        public void AutoMapper_Configuration_IsValid()
        {
            var autoMapper = CreateServer().Host.Services.GetService<IMapper>();
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}