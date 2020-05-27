using System;
using FluentAssertions;
using U.ProductService.Domain.Aggregates.Product.Events;
using Xunit;

namespace U.ProductService.DomainTests.Product.Events
{
    public class ProductAddedDomainEventTest
    {
        [Fact]
        public void Should_Initialize_Correctly()
        {
            //arrange
            Guid id = Guid.NewGuid();
            string name = "eventName";
            decimal price = 999;
            Guid manufacturerId = Guid.NewGuid();
            Guid categoryId = Guid.NewGuid();
            string externalSourceName = "externalSourceName";
            string externalId = Guid.NewGuid().ToString();

            //act
            var @event = new ProductAddedDomainEvent(id, name, price, manufacturerId, categoryId, externalSourceName, externalId);

            //assert
            @event.ProductId.Should().Be(id);
            @event.Name.Should().Be(name);
            @event.Price.Should().Be(price);
            @event.Manufacturer.Should().Be(manufacturerId);
            @event.CategoryId.Should().Be(categoryId);
            @event.ExternalSourceName.Should().Be(externalSourceName);
            @event.ExternalId.Should().Be(externalId);
        }
    }
}