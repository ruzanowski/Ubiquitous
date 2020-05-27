using System;
using FluentAssertions;
using U.ProductService.Domain.Aggregates.Product.Events;
using Xunit;

namespace U.ProductService.DomainTests.Product.Events
{
    public class ProductPriceChangedDomainEventTest
    {
        [Fact]
        public void Should_Initialize_Correctly()
        {
            //arrange
            Guid id = Guid.NewGuid();
            decimal previousPrice = 999;
            decimal currentPrice = 999;

            //act
            var @event = new ProductPriceChangedDomainEvent(id, previousPrice, currentPrice);

            //assert
            @event.ProductId.Should().Be(id);
            @event.PreviousPrice.Should().Be(previousPrice);
            @event.CurrentPrice.Should().Be(currentPrice);
        }
    }
}