using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using U.Common.NetCore.Http;
using U.Common.Pagination;
using U.ProductService.Application.Pictures;
using U.ProductService.Application.Pictures.Commands.AddPicture;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Picture;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Persistance.Repositories.Manufacturer;
using Xunit;

namespace U.ProductService.IntegrationTests.Product
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    [Collection("Sequential")]
    public class ProductTests : UtilitiesBase
    {

        [Fact]
        public async Task Should_CreateProduct_Returns201()
        {
            //arrange
            var command = GetCreateProductCommand();
            //act
            var response = await CreateProductAsync(command);

            //assert
            response.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_GetProductList_Returns200()
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
        public async Task Should_GetProduct_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act
            var httpResponse = await GetProductAsync(createdProduct.Id);
            var stringResult = await httpResponse.Content.ReadAsStringAsync();
            var receivedProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            command.Should().NotBeNull();
            command.Name.Should().Be(createdProduct.Name);
            command.Description.Should().Be(createdProduct.Description);
            command.BarCode.Should().Be(createdProduct.BarCode);
            command.Price.Should().Be(createdProduct.Price);
            command.Dimensions.Should().Be(createdProduct.Dimensions);
            command.ManufacturerId.Should().Be(createdProduct.ManufacturerId);

            receivedProduct.Should().NotBeNull();
            receivedProduct.Id.Should().Be(createdProduct.Id);
            receivedProduct.Name.Should().Be(createdProduct.Name);
            receivedProduct.Description.Should().Be(createdProduct.Description);
            receivedProduct.BarCode.Should().Be(createdProduct.BarCode);
            receivedProduct.Price.Should().Be(createdProduct.Price);
            receivedProduct.Dimensions.Should().Be(createdProduct.Dimensions);
            receivedProduct.ManufacturerId.Should().Be(createdProduct.ManufacturerId);
            receivedProduct.IsPublished.Should().BeFalse();
            receivedProduct.LastUpdatedAt.Should().BeCloseTo(createdProduct.LastUpdatedAt ?? DateTime.UtcNow, TimeSpan.FromSeconds(1));
            receivedProduct.CreatedAt.Should().BeCloseTo(createdProduct.CreatedAt, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task Should_GetProduct_ForNonExistingProduct_Returns404()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var response =  await GetProductAsync(Guid.NewGuid());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_UpdateProduct_Returns200()
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
            var path = $"{ProductController.UpdateProduct()}/{createdProduct.Id}";
            var putResponse = await Client.PutAsJsonAsync(path, updateCommand);

            var getResponse = await GetProductAsync(createdProduct.Id);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

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
        public async Task Should_PublishProduct_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act
            var path = ProductController.PublishProduct(createdProduct.Id);
            var putResponse = await Client.PutAsJsonAsync(path, new {});

            var getResponse = await GetProductAsync(createdProduct.Id);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

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
        public async Task Should_PublishProduct_ForNonExistingProduct_Returns404()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var path = ProductController.PublishProduct(Guid.NewGuid());
            var putResponse = await Client.PutAsJsonAsync(path, new {});

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_UnpublishProduct_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act
            var path = ProductController.UnpublishProduct(createdProduct.Id);
            var putResponse = await Client.PutAsJsonAsync(path, new {});

            var getResponse = await GetProductAsync(createdProduct.Id);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

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
        public async Task Should_UnpublishProduct_ForNonExistingProduct_Returns404()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var path = ProductController.UnpublishProduct(Guid.NewGuid());
            var putResponse = await Client.PutAsJsonAsync(path, new {});

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ChangePriceProduct_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            var createdProduct = await CreateProductAsync(command);

            //act
            var priceArgument = createdProduct.Price + 50;
            var path = ProductController.ChangePriceProduct(createdProduct.Id, (int)priceArgument);
            var putResponse = await Client.PutAsJsonAsync(path, new{});

            var getResponse = await GetProductAsync(createdProduct.Id);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

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
        public async Task<(Guid, Guid)> Should_AddPictureProduct_Returns200()
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
            var addPicturePath = PicturesController.AddPicture();
            var addResponse = await Client.PostAsJsonAsync(addPicturePath, addPictureCommand);
            var addStringResult = await addResponse.Content.ReadAsStringAsync();
            var pictureResult = JsonConvert.DeserializeObject<PictureViewModel>(addStringResult);

            var attachPictureToProduct = ProductController.AttachPictureToProduct(createdProduct.Id, pictureResult.Id);
            var attachResponse = await Client.PostAsJsonAsync(attachPictureToProduct, new {});

            var getResponse = await GetProductAsync(createdProduct.Id);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            addResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            attachResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

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
        public async Task Should_AddPictureProduct_ForNonExistingProduct_Returns404()
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
            var addPicturePath = PicturesController.AddPicture();
            var addResponse = await Client.PostAsJsonAsync(addPicturePath, addPictureCommand);
            var postStringResult = await addResponse.Content.ReadAsStringAsync();
            var pictureResult = JsonConvert.DeserializeObject<PictureViewModel>(postStringResult);

            var attachPictureToProduct = ProductController.AttachPictureToProduct(Guid.NewGuid(), pictureResult.Id);
            var attachResponse = await Client.PostAsJsonAsync(attachPictureToProduct, new {});

            //assert
            attachResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_DeletePicture_Returns200()
        {
            //arrange
            var tuple = await Should_AddPictureProduct_Returns200();

            //act
            var path = PicturesController.DeletePicture(tuple.Item2);
            var putResponse = await Client.DeleteAsync(path);

            await ProductRepository.InvalidateCache(tuple.Item1);

            var getResponse = await GetProductAsync(tuple.Item1);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            responseProduct.Id.Should().Be(tuple.Item1);
            responseProduct.Pictures.Count.Should().Be(0);
        }


        [Fact]
        public async Task Should_DetachPicture_Returns200()
        {
            //arrange
            var tuple = await Should_AddPictureProduct_Returns200();

            //act
            var detachPath = ProductController.DetachPictureFromProduct(tuple.Item1, tuple.Item2);
            var deleteResponse = await Client.DeleteAsync(detachPath);

            var getResponse = await GetProductAsync(tuple.Item1);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            responseProduct.Id.Should().Be(tuple.Item1);
            responseProduct.Pictures.Count.Should().Be(0);
        }


        [Fact]
        public async Task Should_DetachPicture_ForNonExistingProduct_Returns404()
        {
            //arrange
            var tuple = await Should_AddPictureProduct_Returns200();

            //act

            var detachPath = ProductController.DetachPictureFromProduct(Guid.NewGuid(), tuple.Item2);
            var deleteResponse = await Client.DeleteAsync(detachPath);

            var getResponse = await GetProductAsync(tuple.Item1);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseProduct.Pictures.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_DetachPicture_ForNonExistingPicture_Returns404()
        {
            //arrange
            var tuple = await Should_AddPictureProduct_Returns200();

            //act
            var detachPath = ProductController.DetachPictureFromProduct(tuple.Item1, Guid.NewGuid());
            var deleteResponse = await Client.DeleteAsync(detachPath);

            var getResponse = await GetProductAsync(tuple.Item1);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseProduct.Pictures.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_DeletePictureProduct_ForNonExistingTuple_Returns404()
        {
            //arrange
            var tuple = await Should_AddPictureProduct_Returns200();

            //act
            var detachPath = ProductController.DetachPictureFromProduct(Guid.NewGuid(), Guid.NewGuid());
            var deleteResponse = await Client.DeleteAsync(detachPath);

            var getResponse = await GetProductAsync(tuple.Item1);
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var responseProduct = JsonConvert.DeserializeObject<ProductViewModel>(stringResult);

            //assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseProduct.Pictures.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_GetProductStatisticsByManufacturer_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductStatisticsByManufacturer());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<IList<ProductByManufacturersStatisticsDto>>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            statistics.Count.Should().Be(1);
            statistics.First().Count.Should().Be(1);
            statistics.First().ManufacturerName.Should().Be(Manufacturer.GetDraftManufacturer().Name);
        }

        [Fact]
        public async Task Should_GetProductStatisticsByManufacturer_NoProducts_Returns200()
        {
            //arrange

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductStatisticsByManufacturer());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<IList<ProductByManufacturersStatisticsDto>>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            statistics.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_GetProductStatisticsByCategory_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductStatisticsByCategory());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<IList<ProductByCategoryStatisticsDto>>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            statistics.Count.Should().Be(1);
            statistics.First().Count.Should().Be(1);
            statistics.First().CategoryName.Should().Be(ProductCategory.GetDraftCategory().Name);
        }

        [Fact]
        public async Task Should_GetProductStatisticsByCategory_NoProducts_Returns200()
        {
            //arrange

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductStatisticsByCategory());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<IList<ProductByCategoryStatisticsDto>>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            statistics.Should().BeEmpty();
        }


        [Fact]
        public async Task Should_GetProductStatisticsByCreation_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductStatisticsByCreation());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<IList<ProductStatisticsDto>>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            statistics.Count.Should().Be(14);
            statistics.Last().Count.Should().Be(1);
            statistics.Last().DateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromHours(1));
        }

        [Fact]
        public async Task Should_GetProductStatisticsByCreation_NoProducts_Returns200()
        {
            //arrange

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductStatisticsByCreation());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<IList<ProductStatisticsDto>>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            statistics.Should().HaveCount(14);
        }

        [Fact]
        public async Task Should_GetProductCount_Returns200()
        {
            //arrange
            var command = GetCreateProductCommand();
            await CreateProductAsync(command);

            //act
            var getResponse = await Client.GetAsync(ProductController.ProductCount());
            var stringResult = await getResponse.Content.ReadAsStringAsync();
            var count = JsonConvert.DeserializeObject<int>(stringResult);

            //assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            count.Should().Be(1);
        }

        private async Task<HttpResponseMessage> GetProductAsync(Guid id) =>
            await Client.GetAsync(ProductController.GetProduct(id));
    }
}