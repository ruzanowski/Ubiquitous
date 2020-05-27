using System.Linq;
using FluentAssertions;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Domain.SeedWork;
using Xunit;

namespace U.ProductService.DomainTests.Product
{
    public class ProductTypeTest
    {
        [Fact]
        public void Should_OnCreation_HaveCorrectIdAndName()
        {
            //arrange
            const int typeId = 99;
            const string name = "typeName";

            //act
            var productType = new ProductType(typeId, name);

            //assert
            productType.Id.Should().Be(typeId);
            productType.Name.Should().Be(name);
            productType.Id.GetHashCode().Should().Be(typeId.GetHashCode());
            productType.ToString().Should().Be(name);
        }

        [Fact]
        public void Should_OnGetAll_GottenAllProductTypes()
        {
            //arrange
            //act
            var allEnumerations = Enumeration.GetAll<ProductType>();

            //assert
            allEnumerations.Count().Should().Be(3);
        }
    }
}