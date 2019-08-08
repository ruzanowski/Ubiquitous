using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using U.Common.Extensions;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain.Aggregates;
using U.ProductService.IntegrationTests.Products.Conventions;
using Xunit;

namespace U.ProductService.IntegrationTests.Products
{
    public class ProductTests : ProductScenarioBase
    {
        private readonly HttpClient _client;
        private readonly IFixture _fixture;
        
        public ProductTests()
        {
            _client = CreateServer().CreateClient();
            _fixture = new Fixture().Customize(new CreateProductCustomization());
        }
        
        
        [Theory, AutoData]
        public async Task Should_CreateProduct(CreateProductCommand command)
        {
            //act
            //arrange
            var response = await CreateProductAsync(command);
                
            //assert
            response.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task Should_ReturnProductList()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var response = await _client.GetAsync(ProductService.QueryProducts)
                .Result.Content
                .ReadAsJsonAsync<PaginatedItems<ProductViewModel>>();
            
            //assert
            response.Should().BeOfType<PaginatedItems<ProductViewModel>>();
            response.PageSize.Should().Be(25);
            response.PageIndex.Should().Be(0);
            response.Data.Should().NotBeEmpty();
        }
        
        [Theory, AutoData]
        public async Task Should_ReturnProduct(CreateProductCommand command)
        {
            //arrange
            var guid = await CreateProductAsync(command);
            
            //act
            var response = await GetProductAsync(guid);

            //assert
            response.Should().NotBeNull();
            response.Id.Should().Be(guid);
        }
        
        [Fact]
        public async Task Should_UpdateProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var guid = await CreateProductAsync(command);
            var testBody = new CreateProductCommand("testName", "testBarCode", 123, "testDescription",
                new Dimensions(1, 2, 3, 4));
            
            //act
            var path = $"{ProductService.UpdateProduct}/{guid}";
            var response = await _client.PutAsJsonAsync(path, testBody);

            var checkProduct = await GetProductAsync(guid);
            
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            checkProduct.Id.Should().Be(guid);
            checkProduct.Name.Should().Be(testBody.Name);
            checkProduct.Price.Should().Be(testBody.Price);
            checkProduct.BarCode.Should().Be(testBody.BarCode);
            checkProduct.Description.Should().Be(testBody.Description);
            checkProduct.Dimensions.Should().Be(testBody.Dimensions);
        }

        private CreateProductCommand GetCreateProductCommand() => _fixture.Create<CreateProductCommand>();

        private async Task<Guid> CreateProductAsync(CreateProductCommand command)
        {
            var response = await _client.PostAsJsonAsync(ProductService.CreateProduct, command);
            return await response.Content.ReadAsJsonAsync<Guid>();
        }

        private async Task<ProductViewModel> GetProductAsync(Guid guid)
        {
            var path = $"{ProductService.QueryProduct}/{guid}";
            return await _client.GetAsync(path).Result.Content
                .ReadAsJsonAsync<ProductViewModel>();
        }
    }
}