using System;
using FluentAssertions;
using U.ProductService.Domain.Entities.Product.Events;
using Xunit;

namespace U.ProductService.DomainTests.Product.Events
{
    public class ProductUnpublishedDomainEventTest
    {
        [Fact]
        public void Should_Initialize_Correctly()
        {
            //arrange
            Guid id = Guid.NewGuid();
            string name = "eventName";
            decimal price = 999;
            Guid manufacturerId = Guid.NewGuid();

            //act
            var @event = new ProductUnpublishedDomainEvent(id, name, price, manufacturerId);

            //assert
            @event.ProductId.Should().Be(id);
            @event.Name.Should().Be(name);
            @event.Price.Should().Be(price);
            @event.Manufacturer.Should().Be(manufacturerId);
        }
    }
}