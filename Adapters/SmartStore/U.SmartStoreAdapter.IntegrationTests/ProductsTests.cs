using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using U.Common;
using U.SmartStoreAdapter.Api.Products;
using Xunit;

namespace U.SmartStoreAdapter.IntegrationTests
{
    public class ProductTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        
        public ProductTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task ShouldReturnCorrectDataOnListingProducts()
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync(EndpointsAddresses.ProductsAddresses.GetListEndpoint);
            var response =
                JsonConvert.DeserializeObject<PaginatedItems<SmartProductViewModel>>(
                    await httpResponse.Content.ReadAsStringAsync());

            response.Should().BeOfType<PaginatedItems<SmartProductViewModel>>();
            response.PageSize.Should().BePositive();
            response.PageIndex.Should().BePositive();
            response.Data.Should().NotBeNull();
            response.Data.Should().NotBeEmpty();
            response.Data.Should().BeOfType<SmartProductViewModel>();
        }
        
        [Fact]
        public async Task ShouldReturnCorrectDataOnGettingProduct()
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync(EndpointsAddresses.ProductsAddresses.GetListEndpoint);
            var response =
                JsonConvert.DeserializeObject<PaginatedItems<SmartProductViewModel>>(
                    await httpResponse.Content.ReadAsStringAsync());

            response.PageSize.Should().BePositive();
            response.PageIndex.Should().BePositive();
            response.Data.Should().NotBeNull();
            response.Data.Should().NotBeEmpty();
            response.Data.Should().BeOfType<SmartProductViewModel>();
        }
    }
}