using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using U.EventBus.Events.Fetch;
using U.ProductService.Application.Events.IntegrationEvents.EventHandling;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Services;
using Xunit;

namespace U.ProductService.ApplicationTests.EventHandlers
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    [Collection("Sequential")]
    public class NewProductFetchedIntegrationEventHandlerTests : UtilitiesBase
    {
        private readonly NewProductFetchedIntegrationEventHandler _handler;
        private readonly IPendingCommands _pendingCommands;

        public NewProductFetchedIntegrationEventHandlerTests()
        {
            _pendingCommands = Server.Host.Services.CreateScope().ServiceProvider.GetService<IPendingCommands>();
            _handler = Server.Host.Services.CreateScope().ServiceProvider.GetService<NewProductFetchedIntegrationEventHandler>();
        }

        [Fact]
        public async Task Should_NewProductFetched_OnSingleDispatch_DispatchCreate()
        {
            //arrange
            var newProductFetched = new NewProductFetchedIntegrationEvent
            {
                ExternalSourceName = "Fake",
                Id = Guid.NewGuid().ToString(),
                Description = "FakeDescription",
                Height = 1,
                Length = 2,
                Name = "FakeName",
                Price = 3,
                Weight = 4,
                Width = 5,
                BarCode = "fakeBarCode",
                CategoryId = 6,
                IsAvailable = true,
                ManufacturerId = 7,
                StockQuantity = 8
            };

            //act
            await _handler.Handle(newProductFetched);

            //assert
            var createCommands = _pendingCommands.GetCreateCommands();
            var updateCommands = _pendingCommands.GetUpdateCommands();

            createCommands.CreateProductCommands.Should().HaveCount(1);
            updateCommands.UpdateProductCommands.Should().HaveCount(0);
        }

        [Fact]
        public async Task Should_NewProductFetched_OnDoubleDispatch_DispatchCreateAndUpdate()
        {
            //arrange
            var newProductFetched = new NewProductFetchedIntegrationEvent
            {
                ExternalSourceName = "Fake",
                Id = Guid.NewGuid().ToString(),
                Description = "FakeDescription",
                Height = 1,
                Length = 2,
                Name = "FakeName",
                Price = 3,
                Weight = 4,
                Width = 5,
                BarCode = "fakeBarCode",
                CategoryId = 6,
                IsAvailable = true,
                ManufacturerId = 7,
                StockQuantity = 8
            };

            //act
            await _handler.Handle(newProductFetched);

            var command = GetCreateProductCommand();
            command.ExternalProperties = new ExternalCreation
            {
                DuplicationValidated = true,
                SourceId = newProductFetched.Id,
                SourceName = newProductFetched.ExternalSourceName
            };
            await CreateProductAsync(command);

            await _handler.Handle(newProductFetched);

            //assert
            var createCommands = _pendingCommands.GetCreateCommands();
            var updateCommands = _pendingCommands.GetUpdateCommands();

            createCommands.CreateProductCommands.Should().HaveCount(1);
            updateCommands.UpdateProductCommands.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_NewProductFetched_OnSingleDispatch_WhenGivenExists_DispatchCreateAndUpdate()
        {
            //arrange
            var newProductFetched = new NewProductFetchedIntegrationEvent
            {
                ExternalSourceName = "Fake",
                Id = Guid.NewGuid().ToString(),
                Description = "FakeDescription",
                Height = 1,
                Length = 2,
                Name = "FakeName",
                Price = 3,
                Weight = 4,
                Width = 5,
                BarCode = "fakeBarCode",
                CategoryId = 6,
                IsAvailable = true,
                ManufacturerId = 7,
                StockQuantity = 8
            };

            //act
            var command = GetCreateProductCommand();
            command.ExternalProperties = new ExternalCreation
            {
                DuplicationValidated = true,
                SourceId = newProductFetched.Id,
                SourceName = newProductFetched.ExternalSourceName
            };
            await CreateProductAsync(command);

            await _handler.Handle(newProductFetched);

            //assert
            var createCommands = _pendingCommands.GetCreateCommands();
            var updateCommands = _pendingCommands.GetUpdateCommands();

            createCommands.CreateProductCommands.Should().HaveCount(0);
            updateCommands.UpdateProductCommands.Should().HaveCount(1);
        }


    }
}