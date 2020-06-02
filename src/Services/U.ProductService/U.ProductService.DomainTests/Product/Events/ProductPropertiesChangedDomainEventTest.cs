using System;
using FluentAssertions;
using U.ProductService.Domain;
using U.ProductService.Domain.Entities.Product.Events;
using Xunit;

namespace U.ProductService.DomainTests.Product.Events
{
    public class ProductPropertiesChangedDomainEventTest
    {
        [Fact]
        public void Should_Initialize_Correctly()
        {
            //arrange
            Guid productId = Guid.NewGuid();
            string name = "eventName";
            string description = "description";
            decimal price = 999;
            Guid manufacturerId = Guid.NewGuid();
            Dimensions dimensions = new Dimensions(2, 4, 1, 3);

            //act
            var @event = new ProductPropertiesChangedDomainEvent(productId, manufacturerId, name, price, description, dimensions);

            //assert
            @event.ProductId.Should().Be(productId);
            @event.Name.Should().Be(name);
            @event.Price.Should().Be(price);
            @event.Manufacturer.Should().Be(manufacturerId);
            @event.Dimensions.Should().Be(dimensions);
        }
    }
}