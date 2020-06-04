using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using U.Common.NetCore.Http;
using U.ProductService.Application.Pictures.Commands.Add;
using U.ProductService.Application.Pictures.Commands.Update;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Domain.Common;
using Xunit;

namespace U.ProductService.IntegrationTests.Picture
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    [Collection("Sequential")]
    public class PictureTests : UtilitiesBase
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
            var path = PicturesController.AddPicture();
            var putResponse = await Client.PostAsJsonAsync(path, addPictureCommand);

            //assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_DeletePicture_Returns200()
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
            var addPicture = await Client.PostAsJsonAsync(PicturesController.AddPicture(), addPictureCommand);
            var addStringResult = await addPicture.Content.ReadAsStringAsync();
            var pictureViewModel = JsonConvert.DeserializeObject<PictureViewModel>(addStringResult);

            //act
            var path = PicturesController.DeletePicture(pictureViewModel.Id);
            var deleteResponse = await Client.DeleteAsync(path);

            var getPicture = await Client.GetAsync(PicturesController.GetPicture(pictureViewModel.Id));

            //assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            addPicture.StatusCode.Should().Be(HttpStatusCode.Created);
            getPicture.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_UpdatePicture_Returns200()
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
            var addPicture = await Client.PostAsJsonAsync(PicturesController.AddPicture(), addPictureCommand);
            var pictureViewModel = JsonConvert.DeserializeObject<PictureViewModel>(await addPicture.Content.ReadAsStringAsync());

            //act
            var command = new UpdatePictureCommand
            {
                Description = addPictureCommand + " Updated",
                Filename = addPictureCommand + "Updated",
                Url = addPictureCommand + "Updated",
                PictureId = pictureViewModel.Id,
                FileStorageUploadId = Guid.NewGuid(),
                MimeTypeId = MimeType.Bitmap.Id
            };
            var putPicture = await Client.PutAsJsonAsync(PicturesController.UpdatePicture(pictureViewModel.Id), command);


            var getPicture = await Client.GetAsync(PicturesController.GetPicture(pictureViewModel.Id));
            var getResult = JsonConvert.DeserializeObject<PictureViewModel>(await getPicture.Content.ReadAsStringAsync());

            //assert
            addPicture.StatusCode.Should().Be(HttpStatusCode.Created);
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