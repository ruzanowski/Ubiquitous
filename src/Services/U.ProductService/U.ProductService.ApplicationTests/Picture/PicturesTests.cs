using System;
using System.Threading.Tasks;
using FluentAssertions;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Pictures.Commands.AddPicture;
using U.ProductService.Application.Pictures.Commands.DeletePicture;
using U.ProductService.Application.Pictures.Queries.GetPicture;
using U.ProductService.Domain.Common;
using Xunit;

namespace U.ProductService.ApplicationTests.Picture
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    [Collection("Sequential")]
    public class PicturesTests : UtilitiesBase
    {
        [Fact]
        public async Task Should_AddPicture()
        {
            //arrange
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var picture = await Mediator.Send(addPictureCommand);

            //assert
            picture.Id.Should().NotBeEmpty();
            picture.Description.Should().Be(addPictureCommand.Description);
            picture.Url.Should().Be(addPictureCommand.Url);
            picture.FileName.Should().Be(addPictureCommand.Filename);
            picture.MimeTypeId.Should().Be(addPictureCommand.MimeTypeId);
        }


        [Fact]
        public async Task Should_DeletePicture()
        {
            //arrange
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var addPicture = await Mediator.Send(addPictureCommand);

            //act
            await Mediator.Send(new DeletePictureCommand(addPicture.Id));

            Func<Task> task = async () => await Mediator.Send(new GetPictureQuery(addPicture.Id));

            //assert
            addPicture.Id.Should().NotBeEmpty();
            addPicture.Description.Should().Be(addPictureCommand.Description);
            addPicture.Url.Should().Be(addPictureCommand.Url);
            addPicture.FileName.Should().Be(addPictureCommand.Filename);
            addPicture.MimeTypeId.Should().Be(addPictureCommand.MimeTypeId);

            task.Should().Throw<PictureNotFoundException>();
        }

        [Fact]
        public async Task Should_GetPicture()
        {
            //arrange
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var addPicture = await Mediator.Send(addPictureCommand);

            //act
            var getPicture = await Mediator.Send(new GetPictureQuery(addPicture.Id));

            //assert
            addPicture.Id.Should().NotBeEmpty();
            addPicture.Description.Should().Be(addPictureCommand.Description);
            addPicture.Url.Should().Be(addPictureCommand.Url);
            addPicture.FileName.Should().Be(addPictureCommand.Filename);
            addPicture.MimeTypeId.Should().Be(addPictureCommand.MimeTypeId);

            getPicture.Id.Should().Be(addPicture.Id);
            getPicture.Description.Should().Be(addPicture.Description);
            getPicture.Url.Should().Be(addPicture.Url);
            getPicture.FileName.Should().Be(addPicture.FileName);
            getPicture.MimeTypeId.Should().Be(addPicture.MimeTypeId);
            getPicture.FileStorageUploadId.Should().Be(addPicture.FileStorageUploadId);

        }

        [Fact]
        public async Task Should_UpdatePicture()
        {
            //arrange
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var addPicture = await Mediator.Send(addPictureCommand);

            //act
            var command = new UpdatePictureCommand
            {
                PictureId = addPicture.Id,
                Description = addPictureCommand + " Updated",
                Filename = addPictureCommand + "Updated",
                Url = addPictureCommand + "Updated",
                FileStorageUploadId = Guid.NewGuid(),
                MimeTypeId = MimeType.Bitmap.Id
            };
            var update = await Mediator.Send(command);
            var getPicture = await Mediator.Send(new GetPictureQuery(addPicture.Id));
            //assert

            getPicture.Id.Should().Be(update.Id);
            getPicture.Description.Should().Be(update.Description);
            getPicture.Url.Should().Be(update.Url);
            getPicture.FileName.Should().Be(update.FileName);
            getPicture.MimeTypeId.Should().Be(update.MimeTypeId);
            getPicture.FileStorageUploadId.Should().Be(update.FileStorageUploadId);

            getPicture.Id.Should().Be(command.PictureId);
            getPicture.Description.Should().Be(command.Description);
            getPicture.Url.Should().Be(command.Url);
            getPicture.FileName.Should().Be(command.Filename);
            getPicture.MimeTypeId.Should().Be(command.MimeTypeId);
            getPicture.FileStorageUploadId.Should().Be(command.FileStorageUploadId);
        }

        [Fact]
        public async Task Should_UpdatePicture_ThrownPictureNotFoundException()
        {
            //arrange
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };

            //act
            var addPicture = await Mediator.Send(addPictureCommand);

            //act
            var command = new UpdatePictureCommand
            {
                Description = addPictureCommand + " Updated",
                Filename = addPictureCommand + "Updated",
                Url = addPictureCommand + "Updated",
                PictureId = addPicture.Id,
                FileStorageUploadId = Guid.NewGuid(),
                MimeTypeId = MimeType.Bitmap.Id
            };
            var update = await Mediator.Send(command);
            Func<Task> task = async () => await Mediator.Send(new GetPictureQuery(Guid.NewGuid()));
            //assert

            task.Should().Throw<PictureNotFoundException>();
        }
    }
}