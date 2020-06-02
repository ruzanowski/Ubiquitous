using System;
using FluentAssertions;
using U.ProductService.Domain.Entities.Product;
using Xunit;

namespace U.ProductService.DomainTests.Category
{
    public class CategoryTest
    {
        private readonly string _name = "eventName";
        private readonly string _description = "description";
        private readonly Guid _categoryId = Guid.NewGuid();
        private readonly Guid _parentCategoryId = Guid.NewGuid();

        [Fact]
        public void Should_OnCreation_CorrectlyAssignArguments()
        {
            //arrange
            //act
            var product = GetCategory();

            //assert
            product.Id.Should().Be(_categoryId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.ParentCategoryId.Should().Be(_parentCategoryId);
        }

        [Fact]
        public void Should_OnCreationDraftManufacturer_CorrectlyAssignArgumentsAndRaiseProductAddedDomainEvent()
        {
            //arrange
            //act
            var product = ProductCategory.GetDraftCategory();

            //assert
            product.Should().NotBeNull();
        }

        private ProductCategory GetCategory() =>
            new ProductCategory(
                _categoryId,
                _name,
                _description,
                _parentCategoryId
                );
    }
}