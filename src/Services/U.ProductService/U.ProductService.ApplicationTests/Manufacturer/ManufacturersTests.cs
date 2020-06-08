using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.Miscellaneous;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Manufacturers.Commands.AttachPicture;
using U.ProductService.Application.Manufacturers.Commands.Create;
using U.ProductService.Application.Manufacturers.Commands.DetachPicture;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Application.Manufacturers.Queries.GetCount;
using U.ProductService.Application.Manufacturers.Queries.GetList;
using U.ProductService.Application.Manufacturers.Queries.GetSingle;
using Xunit;

namespace U.ProductService.ApplicationTests.Manufacturer
{
    [Collection("Sequential")]
    public class ManufacturerTests : UtilitiesTestBase
    {

        [Fact]
        public async Task Should_GetList_Returns200()
        {
            const int pageSize = 25;
            const int pageIndex = 0;

            //arrange
            //act
            var manufacturers = await Mediator.Send(new GetManufacturersListQuery());

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
        public async Task Should_GetSingle_NotExisting_ThrownManufacturerNotFoundException()
        {
            //arrange
            await CreateManufacturer();

            //act
            Func<Task> task = async () => await Mediator.Send(new QueryManufacturer(Guid.NewGuid()));

            task.Should().Throw<ManufacturerNotFoundException>();
        }

        [Fact]
        public async Task Should_AttachPicture_Returns200()
        {
            //arrange
            var addedPicture = await AddPicture();
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            await Mediator.Send(new AttachPictureToManufacturerCommand(manufacturerBeforeAttachedPicture.Id, addedPicture.Id));
            var manufacturerAfterAttachedPicture = await GetManufacturer(manufacturerBeforeAttachedPicture.Id);

            //assert
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
        public async Task Should_AttachPicture_NotExisting_ThrownPictureNotFoundException()
        {
            //arrange
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            Func<Task> task = async () => await Mediator.Send(new AttachPictureToManufacturerCommand(manufacturerBeforeAttachedPicture.Id, Guid.NewGuid()));

            //assert
            task.Should().Throw<PictureNotFoundException>();
        }

        [Fact]
        public async Task Should_DetachPicture_Returns200()
        {
            //arrange
            var addedPicture = await AddPicture();
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            await Mediator.Send(new AttachPictureToManufacturerCommand(manufacturerBeforeAttachedPicture.Id, addedPicture.Id));
            var manufactureBeforeDetachedPicture = await GetManufacturer(manufacturerBeforeAttachedPicture.Id);


            await Mediator.Send(new DetachPictureFromManufacturerCommand(manufacturerBeforeAttachedPicture.Id, addedPicture.Id));
            var manufacturerAfterDetachedPicture = await GetManufacturer(manufacturerBeforeAttachedPicture.Id);

            //assert
            manufacturerBeforeAttachedPicture.Id.Should().Be(manufacturerBeforeAttachedPicture.Id);
            manufacturerBeforeAttachedPicture.Description.Should().Be(manufacturerBeforeAttachedPicture.Description);
            manufacturerBeforeAttachedPicture.Name.Should().Be(manufacturerBeforeAttachedPicture.Name);
            manufacturerBeforeAttachedPicture.Pictures.Should().HaveCount(0);

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
        public async Task Should_DetachPicture_ProductNotExisting_ThrownPictureNotFound()
        {
            //arrange
            var addedPicture = await AddPicture();
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            await Mediator.Send(new AttachPictureToManufacturerCommand(manufacturerBeforeAttachedPicture.Id, addedPicture.Id));


            Func<Task> task = async () => await Mediator.Send(new DetachPictureFromManufacturerCommand(Guid.NewGuid(), addedPicture.Id));

            //assert
            task.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task Should_DetachPicture_PictureNotExisting_ThrownPictureNotFound()
        {
            //arrange
            var addedPicture = await AddPicture();
            var manufacturerBeforeAttachedPicture = await CreateManufacturer();

            //act
            await Mediator.Send(new AttachPictureToManufacturerCommand(manufacturerBeforeAttachedPicture.Id, addedPicture.Id));

            Func<Task> task = async () => await Mediator.Send(new DetachPictureFromManufacturerCommand(manufacturerBeforeAttachedPicture.Id, Guid.NewGuid()));

            //assert
            task.Should().Throw<PictureNotFoundException>();
        }

        [Fact]
        public async Task Should_GetCount_Returns200()
        {
            //arrange
            var manufacturers = await Mediator.Send(new GetManufacturersListQuery());
            var listCount = manufacturers.Data.Count();

            //act
            var manufacturerCount = await Mediator.Send(new GetManufacturersCount());

            //assert
            manufacturerCount.Should().Be(listCount);
        }

        private async Task<ManufacturerViewModel> CreateManufacturer()
        {
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var command = new CreateManufacturerCommand(name, description);

            var manufacturer = await Mediator.Send(command);
            manufacturer.Id.Should().NotBeEmpty();

            return manufacturer;
        }

        private async Task<ManufacturerViewModel> GetManufacturer(Guid id)
        {
            var manufacturer = await Mediator.Send(new QueryManufacturer(id));

            manufacturer.Id.Should().NotBeEmpty();

            return manufacturer;
        }
    }
}