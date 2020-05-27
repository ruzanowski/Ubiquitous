using System.Linq;
using FluentAssertions;
using U.ProductService.Domain.Aggregates.Picture;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Domain.SeedWork;
using Xunit;

namespace U.ProductService.DomainTests.Pictures
{
    public class MimeTypeTest
    {
        [Fact]
        public void Should_OnCreation_HaveCorrectIdAndName()
        {
            //arrange
            const int typeId = 99;
            const string name = "typeName";

            //act
            var productType = new MimeType(typeId, name);

            //assert
            productType.Id.Should().Be(typeId);
            productType.Name.Should().Be(name);
        }

        [Fact]
        public void Should_OnGetAll_GottenAllMimeTypes()
        {
            var allEnumerations = Enumeration.GetAll<MimeType>();
            allEnumerations.Count().Should().Be(4);
        }
    }
}