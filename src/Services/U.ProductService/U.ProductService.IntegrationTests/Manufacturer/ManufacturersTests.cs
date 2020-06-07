using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.Miscellaneous;
using U.Common.NetCore.Http;
using U.Common.Pagination;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Manufacturers.Commands.Create;
using U.ProductService.Application.Manufacturers.Models;
using Xunit;

namespace U.ProductService.IntegrationTests.Manufacturer
{
    [Collection("Integration_Sequential")]
    public class ManufacturerTests : UtilitiesTestBase
    {

        [Fact]
        public async Task Should_GetList_Returns200()
        {
            //arrange
            const int pageSize = 25;
            const int pageIndex = 0;

            //act
            var httpResponse =  await Client.GetAsync(ManufacturerController.GetList());
            var manufacturers = await httpResponse
                .Content
                .ReadAsJsonAsync<PaginatedItems<CategoryViewModel>>();

            //assert
            manufacturers.PageSize.Should().Be(pageSize);
            manufacturers.PageIndex.Should().Be(pageIndex);
            manufacturers.Data.Should().HaveCount(GlobalConstants.ProductServiceManufacturersSeeded);
        }

        [Fact]
        public async Task Should_Create_Returns201()
        {
            //arrange
            //act
            var manufacturerViewModel = await CreateManufacturer();

            //assert
            manufacturerViewModel.Id.Should().NotBeEmpty();
            manufacturerViewModel.Description.Should().NotBeEmpty();
            manufacturerViewModel.Name.Should().NotBeEmpty();
            manufacturerViewModel.Pictures.Should().HaveCount(0);
        }

        [Fact]
        public async Task Should_GetSingle_Returns200()
        {
            //arrange
            var addedManufacturer = await CreateManufacturer();

            //act
            var manufacturerViewModel = await GetManufacturer(addedManufacturer.Id);

            manufacturerViewModel.Id.Should().Be(addedManufacturer.Id);
            manufacturerViewModel.Description.Should().Be(manufacturerViewModel.Description);
            manufacturerViewModel.Name.Should().Be(manufacturerViewModel.Name);
            manufacturerViewModel.Pictures.Count.Should().Be(manufacturerViewModel.Pictures.Count);
        }

        [Fact]
        public async Task Should_AttachPicture_Returns200()
        {
            //arrange
            var addedPicture = await AddPicture();
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            var attachResponse = await Client.PostAsJsonAsync(ManufacturerController.AttachPicture(manufacturerBeforeAttachedPicture.Id, addedPicture.Id),
                new { });
            var manufacturerAfterAttachedPicture = await GetManufacturer(manufacturerBeforeAttachedPicture.Id);

            //assert
            attachResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            manufacturerBeforeAttachedPicture.Id.Should().NotBeEmpty();
            manufacturerBeforeAttachedPicture.Description.Should().NotBeEmpty();
            manufacturerBeforeAttachedPicture.Name.Should().NotBeEmpty();
            manufacturerBeforeAttachedPicture.Pictures.Should().HaveCount(0);

            manufacturerAfterAttachedPicture.Id.Should().NotBeEmpty();
            manufacturerAfterAttachedPicture.Description.Should().NotBeEmpty();
            manufacturerAfterAttachedPicture.Name.Should().NotBeEmpty();
            manufacturerAfterAttachedPicture.Pictures.Should().HaveCount(1);

            var picture = manufacturerAfterAttachedPicture.Pictures.First();
            picture.Id.Should().Be(addedPicture.Id);
            picture.Description.Should().Be(addedPicture.Description);
            picture.Url.Should().Be(addedPicture.Url);
            picture.FileName.Should().Be(addedPicture.FileName);
            picture.MimeTypeId.Should().Be(addedPicture.MimeTypeId);
            picture.PictureAddedAt.Should().BeCloseTo(addedPicture.PictureAddedAt, TimeSpan.FromSeconds(1));
            picture.FileStorageUploadId.Should().Be(addedPicture.FileStorageUploadId);
        }

        [Fact]
        public async Task Should_DetachPicture_Returns200()
        {
            //arrange
            var addedPicture = await AddPicture();
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            var attachResponse = await Client.PostAsJsonAsync(ManufacturerController.AttachPicture(manufacturerBeforeAttachedPicture.Id, addedPicture.Id),
                new { });
            var manufactureBeforeDetachedPicture = await GetManufacturer(manufacturerBeforeAttachedPicture.Id);


            var detachedResponse = await Client.DeleteAsync(ManufacturerController.DetachPicture(manufacturerBeforeAttachedPicture.Id, addedPicture.Id));
            var manufacturerAfterDetachedPicture = await GetManufacturer(manufacturerBeforeAttachedPicture.Id);

            //assert
            attachResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            detachedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            manufacturerBeforeAttachedPicture.Id.Should().Be(manufacturerBeforeAttachedPicture.Id);
            manufacturerBeforeAttachedPicture.Description.Should().Be(manufacturerBeforeAttachedPicture.Description);
            manufacturerBeforeAttachedPicture.Name.Should().Be(manufacturerBeforeAttachedPicture.Name);
            manufacturerBeforeAttachedPicture.Pictures.Should().HaveCount(0);

            detachedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            manufactureBeforeDetachedPicture.Id.Should().Be(manufacturerBeforeAttachedPicture.Id);
            manufactureBeforeDetachedPicture.Description.Should().Be(manufacturerBeforeAttachedPicture.Description);
            manufactureBeforeDetachedPicture.Name.Should().Be(manufacturerBeforeAttachedPicture.Name);
            manufactureBeforeDetachedPicture.Pictures.Should().HaveCount(1);

            var picture = manufactureBeforeDetachedPicture.Pictures.First();
            picture.Id.Should().Be(addedPicture.Id);
            picture.Description.Should().Be(addedPicture.Description);
            picture.Url.Should().Be(addedPicture.Url);
            picture.FileName.Should().Be(addedPicture.FileName);
            picture.MimeTypeId.Should().Be(addedPicture.MimeTypeId);
            picture.PictureAddedAt.Should().BeCloseTo(addedPicture.PictureAddedAt, TimeSpan.FromSeconds(1));
            picture.FileStorageUploadId.Should().Be(addedPicture.FileStorageUploadId);

            manufacturerAfterDetachedPicture.Id.Should().Be(manufacturerBeforeAttachedPicture.Id);
            manufacturerAfterDetachedPicture.Description.Should().Be(manufacturerBeforeAttachedPicture.Description);
            manufacturerAfterDetachedPicture.Name.Should().Be(manufacturerBeforeAttachedPicture.Name);
            manufacturerAfterDetachedPicture.Pictures.Should().HaveCount(0);
        }

        [Fact]
        public async Task Should_GetCount_Returns200()
        {
            //arrange
            var httpResponseList =  await Client.GetAsync(ManufacturerController.GetList());
            var manufacturers = await httpResponseList
                .Content
                .ReadAsJsonAsync<PaginatedItems<ManufacturerViewModel>>();
            var listCount = manufacturers.Data.Count();

            //act
            var httpResponseCount =  await Client.GetAsync(ManufacturerController.Count());
            var manufacturersCount = await httpResponseCount
                .Content
                .ReadAsJsonAsync<int>();

            //assert
            manufacturersCount.Should().Be(listCount);
        }

        private async Task<ManufacturerViewModel> CreateManufacturer()
        {
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var command = new CreateManufacturerCommand(name, description);

            const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;

            var response = await Client.PostAsJsonAsync(ManufacturerController.Create(), command);
            response.StatusCode.Should().Be(expectedStatusCode);

            return await response.Content.ReadAsJsonAsync<ManufacturerViewModel>();
        }

        private async Task<ManufacturerViewModel> GetManufacturer(Guid id)
        {
            var httpResponse = await Client.GetAsync(ManufacturerController.Get(id));
            var manufacturer = await httpResponse
                .Content
                .ReadAsJsonAsync<ManufacturerViewModel>();

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            return manufacturer;
        }
    }
}