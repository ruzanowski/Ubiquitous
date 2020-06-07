using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.Miscellaneous;
using U.Common.NetCore.Http;
using U.Common.Pagination;
using U.ProductService.Application.Categories.Commands.Create;
using U.ProductService.Application.Categories.Models;
using Xunit;

namespace U.ProductService.IntegrationTests.Category
{
    [Collection("Integration_Sequential")]
    public class CategoriesTests : UtilitiesTestBase
    {

        [Fact]
        public async Task Should_GetList_Returns200()
        {
            const int pageSize = 25;
            const int pageIndex = 0;

            //arrange
            //act
            var httpResponse =  await Client.GetAsync(CategoryController.GetList());
            var categories = await httpResponse
                .Content
                .ReadAsJsonAsync<PaginatedItems<CategoryViewModel>>();
            //assert
            categories.PageSize.Should().Be(pageSize);
            categories.PageIndex.Should().Be(pageIndex);
            categories.Data.Should().HaveCount(GlobalConstants.ProductServiceCategoriesSeeded);
        }

        [Fact]
        public async Task Should_Create_Returns201()
        {
            //arrange
            //act
            var categoryViewModel = await CreateCategory();

            //assert
            categoryViewModel.Id.Should().NotBeEmpty();
            categoryViewModel.Description.Should().NotBeEmpty();
            categoryViewModel.Name.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_GetSingle_Returns200()
        {
            //arrange
            var addedCategory = await CreateCategory();

            //act
            var categoryViewModel = await GetCategory(addedCategory.Id);

            categoryViewModel.Id.Should().Be(addedCategory.Id);
            categoryViewModel.Description.Should().Be(categoryViewModel.Description);
            categoryViewModel.Name.Should().Be(categoryViewModel.Name);
        }

        [Fact]
        public async Task Should_GetSingle_Returns404()
        {
            //arrange
            //act
            var httpResponse = await Client.GetAsync(CategoryController.Get(Guid.NewGuid()));

            //assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_GetCount_Returns200()
        {
            //arrange
            var httpResponseList =  await Client.GetAsync(CategoryController.GetList());
            var category = await httpResponseList
                .Content
                .ReadAsJsonAsync<PaginatedItems<CategoryViewModel>>();
            var listCount = category.Data.Count();

            //act
            var httpResponseCount =  await Client.GetAsync(CategoryController.Count());
            var categoryCount = await httpResponseCount
                .Content
                .ReadAsJsonAsync<int>();

            //assert
            categoryCount.Should().Be(listCount);
        }

        private async Task<CategoryViewModel> CreateCategory()
        {
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var command = new CreateCategoryCommand(name, description);

            const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;

            var response = await Client.PostAsJsonAsync(CategoryController.Create(), command);
            response.StatusCode.Should().Be(expectedStatusCode);

            return await response.Content.ReadAsJsonAsync<CategoryViewModel>();
        }

        private async Task<CategoryViewModel> GetCategory(Guid id)
        {
            var httpResponse = await Client.GetAsync(CategoryController.Get(id));
            var category = await httpResponse
                .Content
                .ReadAsJsonAsync<CategoryViewModel>();

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            return category;
        }
    }
}