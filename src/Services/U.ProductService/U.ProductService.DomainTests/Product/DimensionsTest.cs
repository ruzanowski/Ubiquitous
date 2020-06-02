using FluentAssertions;
using U.ProductService.Domain;
using Xunit;

namespace U.ProductService.DomainTests.Product
{
    public class DimensionsTest
    {
        [Fact]
        public void Should_OnCreation_ShouldHaveCorrectFields()
        {
            //arrange
            const int length = 1;
            const int width = 2;
            const int height = 3;
            const int weight = 4;

            //act
            var dimensions = new Dimensions(length, width, height, weight);

            //assert
            dimensions.Length.Should().Be(length);
            dimensions.Width.Should().Be(width);
            dimensions.Height.Should().Be(height);
            dimensions.Weight.Should().Be(weight);
        }

        [Fact]
        public void Should_OnGetCopy_ShouldHaveCorrectCopiedParameters()
        {
            //arrange
            const int length = 1;
            const int width = 2;
            const int height = 3;
            const int weight = 4;
            var dimensions = new Dimensions(length, width, height, weight);

            //act
            var copy = dimensions.GetCopy() as Dimensions;

            //assert
            copy!.Length.Should().Be(length);
            copy!.Width.Should().Be(width);
            copy!.Height.Should().Be(height);
            copy!.Weight.Should().Be(weight);
        }
    }
}