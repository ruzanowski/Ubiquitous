using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using U.Common.NetCore.Http;
using U.ProductService.Application.Pictures.Commands.Update;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Domain.Common;
using Xunit;

namespace U.ProductService.IntegrationTests.Picture
{
    [Collection("Integration_Sequential")]
    public class PictureTests : UtilitiesTestBase
    {
        [Fact]
        public async Task Should_AddPicture()
        {
            //arrange
            var pictureAdded = await AddPicture();

            //assert
            pictureAdded.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_DeletePicture_Returns200()
        {
            //arrange
            var pictureAdded = await AddPicture();

            //act
            var path = PicturesController.DeletePicture(pictureAdded.Id);
            var deleteResponse = await Client.DeleteAsync(path);

            var getPicture = await Client.GetAsync(PicturesController.GetPicture(pictureAdded.Id));

            //assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getPicture.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_UpdatePicture_Returns200()
        {
            //arrange
            var pictureAdded = await AddPicture();

            //act
            var command = new UpdatePictureCommand
            {
                Description = pictureAdded.Description + " Updated",
                Filename = pictureAdded.FileName + "Updated",
                Url = pictureAdded.Url + "Updated",
                PictureId = pictureAdded.Id,
                FileStorageUploadId = Guid.NewGuid(),
                MimeTypeId = MimeType.Bitmap.Id
            };
            var putPicture = await Client.PutAsJsonAsync(PicturesController.UpdatePicture(pictureAdded.Id), command);


            var getPicture = await Client.GetAsync(PicturesController.GetPicture(pictureAdded.Id));
            var getResult = JsonConvert.DeserializeObject<PictureViewModel>(await getPicture.Content.ReadAsStringAsync());

            //assert
            putPicture.StatusCode.Should().Be(HttpStatusCode.OK);
            getPicture.StatusCode.Should().Be(HttpStatusCode.OK);

            getResult.Id.Should().Be(command.PictureId);
            getResult.Description.Should().Be(command.Description);
            getResult.Url.Should().Be(command.Url);
            getResult.FileName.Should().Be(command.Filename);
            getResult.MimeTypeId.Should().Be(command.MimeTypeId);
            getResult.FileStorageUploadId.Should().Be(command.FileStorageUploadId);
        }
    }
}