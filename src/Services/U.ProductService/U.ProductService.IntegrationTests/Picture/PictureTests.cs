using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using U.Common.NetCore.Http;
using U.ProductService.Application.Pictures.Commands.AddPicture;
using U.ProductService.Domain.Common;
using Xunit;

namespace U.ProductService.IntegrationTests.PictureTests
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
    }
}