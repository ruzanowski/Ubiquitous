using System;
using FluentAssertions;
using Newtonsoft.Json;
using U.ProductService.Domain.Aggregates.Product.Events;
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
            product.AggregateId.Should().Be(_categoryId);
            product.Id.Should().Be(_categoryId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Category.Category));
            product.ParentCategoryId.Should().Be(_parentCategoryId);
            product.IsDraft.Should().BeFalse();
        }

        [Fact]
        public void Should_OnCreationDraftManufacturer_CorrectlyAssignArgumentsAndRaiseProductAddedDomainEvent()
        {
            //arrange
            //act
            var product = Domain.Aggregates.Category.Category.GetDraftCategory();

            //assert
            product.Should().NotBeNull();
        }

        private Domain.Aggregates.Category.Category GetCategory() =>
            new Domain.Aggregates.Category.Category(
                _categoryId,
                _name,
                _description,
                _parentCategoryId
                );
    }
}