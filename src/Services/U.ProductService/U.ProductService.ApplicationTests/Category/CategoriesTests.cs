using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.Miscellaneous;
using U.ProductService.Application.Categories.Commands.Create;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Categories.Queries.GetCategories;
using U.ProductService.Application.Categories.Queries.GetCategoriesCount;
using U.ProductService.Application.Categories.Queries.GetCategory;
using U.ProductService.Application.Common.Exceptions;
using Xunit;

namespace U.ProductService.ApplicationTests.Category
{
    [Collection("Sequential")]
    public class CategoriesTests : UtilitiesTestBase
    {

        [Fact]
        public async Task Should_GetList_Returns200()
        {
            //arrange
            const int pageSize = 25;
            const int pageIndex = 0;

            //act
            var categories = await Mediator.Send(new GetCategoriesListQuery());

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
        public async Task Should_GetSingle_NotExisting_ThrownCategoryNotFoundException()
        {
            //arrange
            //act
            Func<Task> task = async () => await Mediator.Send(new GetCategoryQuery(Guid.NewGuid()));

            //assert
            task.Should().Throw<CategoryNotFoundException>();
        }

        [Fact]
        public async Task Should_GetCount_Returns200()
        {
            //arrange
            var categories =  await Mediator.Send(new GetCategoriesListQuery());
            var listCount = categories.Data.Count();

            //act
            var categoryCount = await Mediator.Send(new GetCategoriesCount());

            //assert
            categoryCount.Should().Be(listCount);
        }

        private async Task<CategoryViewModel> CreateCategory()
        {
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var command = new CreateCategoryCommand(name, description);

            var response = await Mediator.Send(command);

            return response;
        }

        private async Task<CategoryViewModel> GetCategory(Guid id)
        {
            var category = await Mediator.Send(new GetCategoryQuery(id));

            return category;
        }
    }
}