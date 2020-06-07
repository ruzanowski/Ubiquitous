using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.Pagination;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Pictures.Commands.Add;
using U.ProductService.Application.Pictures.Commands.Delete;
using U.ProductService.Application.Products.Commands.AttachPicture;
using U.ProductService.Application.Products.Commands.ChangePrice;
using U.ProductService.Application.Products.Commands.DetachPicture;
using U.ProductService.Application.Products.Commands.Publish;
using U.ProductService.Application.Products.Commands.UnPublish;
using U.ProductService.Application.Products.Commands.Update.Single;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.GetCount;
using U.ProductService.Application.Products.Queries.GetSingle;
using U.ProductService.Application.Products.Queries.GetStatistics;
using U.ProductService.Application.Products.Queries.GetStatisticsByCategory;
using U.ProductService.Application.Products.Queries.GetStatisticsByManufacturers;
using U.ProductService.Domain.Common;
using Xunit;

namespace U.ProductService.ApplicationTests.Product
{
    [Collection("Application_Sequential")]
    public class ProductTests : UtilitiesTestBase
    {

        [Fact]
        public async Task Should_CreateProduct()
        {
            //arrange
            var command = GetCreateProductCommand();

            //act
            var response = await CreateProductAsync(command);

            //assert
            response.Id.Should().NotBeEmpty();
            response.Name.Should().Be(command.Name);
            response.Description.Should().Be(command.Description);
            response.BarCode.Should().Be(command.BarCode);
            response.Price.Should().Be(command.Price);
            response.Dimensions.Should().Be(command.Dimensions);
            response.ManufacturerId.Should().Be(command.ManufacturerId!.Value);
        }

        [Fact]
        public async Task Should_GetProductList()
        {
            //arrange
            var command = GetCreateProductCommand();
            const int pageSize = 25;
            const int pageIndex = 0;
            const int dataCount = 1;
            var createdProduct = await CreateProductAsync(command);

            //act
            var response = await GetProductsAsync();

            //assert
            response.Should().BeOfType<PaginatedItems<ProductViewModel>>();
            response.PageSize.Should().Be(pageSize);
            response.PageIndex.Should().Be(pageIndex);
            response.Data.Count().Should().Be(dataCount);

            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            var product = response.Data.First();
            product.Should().NotBeNull();
            product.Id.Should().Be(createdProduct.Id);
            product.Name.Should().Be(createdProduct.Name);
            product.Description.Should().Be(createdProduct.Description);
            product.BarCode.Should().Be(createdProduct.BarCode);
            product.Price.Should().Be(createdProduct.Price);
            product.Dimensions.Should().Be(createdProduct.Dimensions);
            product.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            product.IsPublished.Should().BeFalse();
        }

        [Fact]
        public async Task Should_GetProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act
            var getProduct = await GetProductAsync(createdProduct.Id);

            //assert

            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            getProduct.Should().NotBeNull();
            getProduct.Id.Should().Be(createdProduct.Id);
            getProduct.Name.Should().Be(createdProduct.Name);
            getProduct.Description.Should().Be(createdProduct.Description);
            getProduct.BarCode.Should().Be(createdProduct.BarCode);
            getProduct.Price.Should().Be(createdProduct.Price);
            getProduct.Dimensions.Should().Be(createdProduct.Dimensions);
            getProduct.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            getProduct.IsPublished.Should().BeFalse();
            getProduct.LastUpdatedAt.Should().BeCloseTo(createdProduct.LastUpdatedAt ?? DateTime.UtcNow, TimeSpan.FromSeconds(1));
            getProduct.CreatedAt.Should().BeCloseTo(createdProduct.CreatedAt, TimeSpan.FromSeconds(1));
        }


        [Fact]
        public async Task Should_CreateProduct_OnDuplicated_ThrownProductDuplicatedException()
        {
            //arrange
            var command = GetCreateProductCommand();
            command.ExternalProperties = new ExternalCreation
            {
                DuplicationValidated = false,
                SourceId = Guid.NewGuid().ToString(),
                SourceName = "Fake"
            };
            await CreateProductAsync(command);

            //act
            Func<Task> task = async () => await CreateProductAsync(command);

            //assert
            task.Should().Throw<ProductDuplicatedException>();
        }

        [Fact]
        public async Task Should_GetProduct_ForNonExistingProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            Func<Task> response = async () =>  await GetProductAsync(Guid.NewGuid());

            //assert
            response.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task Should_UpdateProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            var updateCommand = new UpdateProductCommand(createdProduct.Id,
                "testName",
                123,
                "testDescription",
                new DimensionsDto
                {
                    Height = 1.0M,
                    Length = 2.0M,
                    Weight = 3.0M,
                    Width = 4.0M
                });

            //act
            var putResponse = await Mediator.Send(updateCommand);

            var responseProduct = await GetProductAsync(createdProduct.Id);

            //assert

            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            responseProduct.Id.Should().Be(updateCommand.Id);
            responseProduct.Name.Should().Be(updateCommand.Name);
            responseProduct.Price.Should().Be(updateCommand.Price);
            responseProduct.Description.Should().Be(updateCommand.Description);
            responseProduct.Dimensions.Should().Be(updateCommand.Dimensions);
        }

        [Fact]
        public async Task Should_PublishProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act
            await Mediator.Send(new PublishProductCommand(createdProduct.Id));
            var responseProduct = await GetProductAsync(createdProduct.Id);

            //assert

            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            responseProduct.Id.Should().Be(createdProduct.Id);
            responseProduct.Name.Should().Be(createdProduct.Name);
            responseProduct.Description.Should().Be(createdProduct.Description);
            responseProduct.BarCode.Should().Be(createdProduct.BarCode);
            responseProduct.Price.Should().Be(createdProduct.Price);
            responseProduct.Dimensions.Should().Be(createdProduct.Dimensions);
            responseProduct.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            responseProduct.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task Should_PublishProduct_ForNonExistingProduct_ThrowsProductNotFoundException()
        {
            //arrange
            await CreateProductAsync(GetCreateProductCommand());

            //act
            Func<Task> task = async () => await Mediator.Send(new PublishProductCommand(Guid.NewGuid()));

            //assert
            task.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task Should_UnpublishProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act

            await Mediator.Send(new UnPublishProductCommand(createdProduct.Id));
            var responseProduct = await GetProductAsync(createdProduct.Id);

            //assert

            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            responseProduct.Id.Should().Be(createdProduct.Id);
            responseProduct.Name.Should().Be(createdProduct.Name);
            responseProduct.Description.Should().Be(createdProduct.Description);
            responseProduct.BarCode.Should().Be(createdProduct.BarCode);
            responseProduct.Price.Should().Be(createdProduct.Price);
            responseProduct.Dimensions.Should().Be(createdProduct.Dimensions);
            responseProduct.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            responseProduct.IsPublished.Should().BeFalse();
        }

        [Fact]
        public async Task Should_UnpublishProduct_ForNonExistingProduct_ThrowsProductNotFoundException()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            Func<Task> task = async () => await Mediator.Send(new PublishProductCommand(Guid.NewGuid()));

            //assert
            task.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task Should_ChangePriceProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);
            var priceArgument = createdProduct.Price + 50;

            //act
            await Mediator.Send(new ChangeProductPriceCommand(createdProduct.Id, priceArgument));
            var responseProduct = await GetProductAsync(createdProduct.Id);

            //assert
            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().NotBe(priceArgument);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            responseProduct.Id.Should().Be(createdProduct.Id);
            responseProduct.Name.Should().Be(createdProduct.Name);
            responseProduct.Description.Should().Be(createdProduct.Description);
            responseProduct.BarCode.Should().Be(createdProduct.BarCode);
            responseProduct.Price.Should().Be(priceArgument);
            responseProduct.Dimensions.Should().Be(createdProduct.Dimensions);
            responseProduct.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            responseProduct.IsPublished.Should().BeFalse();
        }

        [Fact]
        public async Task<(Guid, Guid)> Should_AddPictureProduct()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var addPictureResult = await Mediator.Send(addPictureCommand);
            await Mediator.Send(new AttachPictureToProductCommand(createdProduct.Id, addPictureResult.Id));
            var responseProduct = await GetProductAsync(createdProduct.Id);

            //assert
            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            responseProduct.Id.Should().Be(createdProduct.Id);
            responseProduct.Name.Should().Be(createdProduct.Name);
            responseProduct.Description.Should().Be(createdProduct.Description);
            responseProduct.BarCode.Should().Be(createdProduct.BarCode);
            responseProduct.Price.Should().Be(createdProduct.Price);
            responseProduct.Dimensions.Should().Be(createdProduct.Dimensions);
            responseProduct.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            responseProduct.IsPublished.Should().BeFalse();
            responseProduct.Pictures.Count.Should().Be(1);

            var picture = responseProduct.Pictures.First();
            picture.Id.Should().NotBeEmpty();
            picture.Description.Should().Be(addPictureCommand.Description);
            picture.Url.Should().Be(addPictureCommand.Url);
            picture.FileName.Should().Be(addPictureCommand.Filename);
            picture.MimeTypeId.Should().Be(addPictureCommand.MimeTypeId);

            return (responseProduct.Id, picture.Id);
        }

        [Fact]
        public async Task Should_AttachPictureProduct_ForNonExistingProduct_Returns404()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var addPictureResult = await Mediator.Send(addPictureCommand);

            Func<Task> task = async () => await Mediator.Send(new AttachPictureToProductCommand(Guid.NewGuid(), addPictureResult.Id));

            //assert
            task.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task Should_DeletePicture()
        {
            //arrange
            var tuple = await Should_AddPictureProduct();

            //act
            await Mediator.Send(new DeletePictureCommand(tuple.Item2));
            await ProductRepository.InvalidateCacheAsync(tuple.Item1);
            var getResponse = await GetProductAsync(tuple.Item1);

            //assert
            getResponse.Id.Should().Be(tuple.Item1);
            getResponse.Pictures.Count.Should().Be(0);
        }


        [Fact]
        public async Task Should_DetachPicture()
        {
            //arrange
            var tuple = await Should_AddPictureProduct();

            //act
            await Mediator.Send(new DetachPictureToProductCommand(tuple.Item1, tuple.Item2));
            var getResponse = await GetProductAsync(tuple.Item1);

            //assert
            getResponse.Id.Should().Be(tuple.Item1);
            getResponse.Pictures.Count.Should().Be(0);
        }


        [Fact]
        public async Task Should_DetachPicture_ForNonExistingProduct_ThrowsProductNotFoundException()
        {
            //arrange
            var tuple = await Should_AddPictureProduct();

            //act
            Func<Task> task = async () => await Mediator.Send(new DetachPictureToProductCommand(Guid.NewGuid(), tuple.Item2));
            var getResponse = await GetProductAsync(tuple.Item1);

            //assert
            task.Should().Throw<ProductNotFoundException>();
            getResponse.Pictures.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_DetachPicture_ForNonExistingPicture_ThrowsNotFoundDomainException()
        {
            //arrange
            var tuple = await Should_AddPictureProduct();

            //act
            Func<Task> task = async () => await Mediator.Send(new DetachPictureToProductCommand(tuple.Item1, Guid.NewGuid()));
            var getResponse = await GetProductAsync(tuple.Item1);

            //assert
            task.Should().Throw<PictureNotFoundException>();
            getResponse.Pictures.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_DeletePictureProduct_ForNonExistingTuple_ThrowsProductNotFoundException()
        {
            //arrange
            var tuple = await Should_AddPictureProduct();

            //act
            Func<Task> task = async () => await Mediator.Send(new DetachPictureToProductCommand(Guid.NewGuid(), Guid.NewGuid()));
            var getResponse = await GetProductAsync(tuple.Item1);

            //assert
            task.Should().Throw<ProductNotFoundException>();
            getResponse.Pictures.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_GetProductStatisticsByManufacturer()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var statistics = await Mediator.Send(new GetProductsStatisticsByManufacturers());

            //assert
            statistics.Count.Should().Be(1);
            statistics.First().Count.Should().Be(1);
            statistics.First().ManufacturerName.Should().Be(Domain.Entities.Manufacturer.Manufacturer.GetDraftManufacturer().Name);
        }

        [Fact]
        public async Task Should_GetProductStatisticsByManufacturer_NoProducts()
        {
            //arrange

            //act
            var statistics = await Mediator.Send(new GetProductsStatisticsByManufacturers());


            //assert
            statistics.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_GetProductStatisticsByCategory()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var statistics = await Mediator.Send(new GetProductsStatisticsByCategory());

            //assert
            statistics.Count.Should().Be(1);
            statistics.First().Count.Should().Be(1);
            statistics.First().CategoryName.Should().Be(Domain.Entities.Product.Category.GetDraftCategory().Name);
        }

        [Fact]
        public async Task Should_GetProductStatisticsByCategory_NoProducts()
        {
            //arrange

            //act
            var statistics = await Mediator.Send(new GetProductsStatisticsByCategory());


            //assert
            statistics.Should().BeEmpty();
        }


        [Fact]
        public async Task Should_GetProductStatisticsByCreation()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var statistics = await Mediator.Send(new GetProductsStatisticsQuery());

            //assert
            statistics.Count.Should().Be(14);
            statistics.Last().Count.Should().Be(1);
            statistics.Last().DateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromHours(2));
        }

        [Fact]
        public async Task Should_GetProductStatisticsByCreation_NoProducts()
        {
            //arrange

            //act
            var statistics = await Mediator.Send(new GetProductsStatisticsQuery());

            //assert
            statistics.Should().HaveCount(14);
        }

        [Fact]
        public async Task Should_GetProductCount()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var count = await Mediator.Send(new GetProductsCount());

            //assert
            count.Should().Be(1);
        }

        private async Task<ProductViewModel> GetProductAsync(Guid id) =>
            await Mediator.Send(new QueryProduct(id));
    }
}