using System.Threading.Tasks;
using FluentAssertions;
using U.ProductService.Application.Pictures.Commands.AddPicture;
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
            var picture = await _mediator.Send(addPictureCommand);

            //assert
            picture.Id.Should().NotBeEmpty();
            picture.Description.Should().Be(addPictureCommand.Description);
            picture.Url.Should().Be(addPictureCommand.Url);
            picture.FileName.Should().Be(addPictureCommand.Filename);
            picture.MimeTypeId.Should().Be(addPictureCommand.MimeTypeId);
        }
    }
}