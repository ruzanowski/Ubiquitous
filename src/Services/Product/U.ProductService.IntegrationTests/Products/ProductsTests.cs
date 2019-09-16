using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.Extensions;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Models;
using U.ProductService.IntegrationTests.Products.Conventions;
using Xunit;

namespace U.ProductService.IntegrationTests.Products
{
    public class ProductTests : TestBase
    {
        private readonly HttpClient _client;

        public ProductTests()
        {
            _client = CreateServer().CreateClient();
        }

        [Theory]
        [ProductAutoData]
        public async Task Should_CreateProduct(CreateProductCommand command)
        {
            //act
            //arrange
            var response = await CreateProductAsync(command);

            //assert
            response.Should().NotBeEmpty();
        }

        [Theory]
        [ProductAutoData]
        public async Task Should_ReturnProductList(CreateProductCommand command)
        {
            //arrange
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

        [Theory]
        [ProductAutoData]
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

//        [Theory]
//        [ProductAutoData]
//        public async Task Should_UpdateProduct(CreateProductCommand command)
//        {
//            //arrange
//            var guid = await CreateProductAsync(command);
//            var dimensions = new DimensionsDto
//            {
//                Height = 1,
//                Length = 2,
//                Weight = 3,
//                Width = 4
//            };
//
//            var update = new UpdateProductCommand(guid, "testName", 123, "testDescription", dimensions);
//
//            //act
//            var path = $"{ProductService.UpdateProduct}/{guid}";
//            var response = await _client.PutAsJsonAsync(path, update);
//
//            var checkProduct = await GetProductAsync(guid);
//
//            //assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            checkProduct.Id.Should().Be(guid);
//            checkProduct.Name.Should().Be(update.Name);
//            checkProduct.Price.Should().Be(update.Price);
//            checkProduct.Description.Should().Be(update.Description);
//            checkProduct.Dimensions.Should().Be(update.Dimensions);
//        }

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