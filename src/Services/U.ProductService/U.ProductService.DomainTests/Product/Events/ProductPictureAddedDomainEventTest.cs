using System;
using FluentAssertions;
using U.ProductService.Domain.Entities.Product.Events;
using Xunit;

namespace U.ProductService.DomainTests.Product.Events
{
    public class ProductPictureAddedDomainEventTest
    {
        [Fact]
        public void Should_Initialize_Correctly()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Guid pictureId = Guid.NewGuid();

            //act
            var @event = new ProductPictureAddedDomainEvent(id, pictureId);

            //assert
            @event.ProductId.Should().Be(id);
            @event.PictureId.Should().Be(pictureId);
        }
    }
}