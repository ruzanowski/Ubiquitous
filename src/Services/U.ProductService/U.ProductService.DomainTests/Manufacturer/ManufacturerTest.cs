using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Exceptions;
using Xunit;

namespace U.ProductService.DomainTests.Manufacturer
{
    public class ManufacturerTest
    {
        private readonly Guid _manufacturerId = Guid.NewGuid();
        private readonly string _uniqueClientId = Guid.NewGuid().ToString();
        private readonly string _name = "ManufacturerName";
        private readonly string _description = "ManufacturerDescription";

        // [Fact]
        // public void Should_AddPicture_AddedPictureAndRaisedPictureAddedDomainEvent()
        // {
        //     //arrange
        //     Guid pictureId = Guid.NewGuid();
        //     Guid fileStorageId = Guid.NewGuid();
        //     string filename = "seoFilename";
        //     string description = "pictureDescription";
        //     string url = "https://ubiquitous.com/product/picture/123923";
        //     MimeType mimeType = MimeType.Jpg;
        //
        //     var manufacturer = GetManufacturer();
        //     var lastUpdatedAt = manufacturer.LastUpdatedAt;
        //     var lastUpdatedBy = manufacturer.LastUpdatedBy;
        //     var createdAt = manufacturer.CreatedAt;
        //     var createdBy = manufacturer.CreatedBy;
        //
        //     //act
        //     manufacturer.AttachPicture(pictureId,
        //         fileStorageId,
        //         filename,
        //         description,
        //         url,
        //         mimeType);
        //
        //     //arrange
        //     manufacturer.AggregateId.Should().Be(_manufacturerId);
        //     manufacturer.Id.Should().Be(_manufacturerId);
        //     manufacturer.Name.Should().Be(_name);
        //     manufacturer.Description.Should().Be(_description);
        //     manufacturer.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
        //     manufacturer.LastUpdatedBy.Should().Be(lastUpdatedBy);
        //     manufacturer.CreatedAt.Should().Be(createdAt);
        //     manufacturer.CreatedBy.Should().Be(createdBy);
        //     manufacturer.AggregateTypeName.Should().Be(nameof(Domain.Entities.Manufacturer.Manufacturer));
        //     manufacturer.UniqueClientId.Should().Be(_uniqueClientId);
        //     var picture = manufacturer.Pictures.First();
        //     picture.Id.Should().Be(pictureId);
        //     picture.Url.Should().Be(url);
        //     picture.Description.Should().Be(description);
        //     picture.FileName.Should().Be(filename);
        //     picture.MimeType.Should().BeNull();
        //     picture.FileStorageUploadId.Should().Be(fileStorageId);
        // }
        //
        // [Theory]
        // [InlineData("", "pictureDescription", "https://ubiquitous.com/product/picture/123923")]
        // [InlineData(null, "pictureDescription", "https://ubiquitous.com/product/picture/123923")]
        // [InlineData("fileName", "", "https://ubiquitous.com/product/picture/123923")]
        // [InlineData("fileName", null, "https://ubiquitous.com/product/picture/123923")]
        // [InlineData("fileName", "pictureDescription", "")]
        // [InlineData("fileName", "pictureDescription", null)]
        // public void Should_AddPictureWithNullArguments_ThrownDomainException(string filename, string description, string url)
        // {
        //     //arrange
        //     Guid pictureId = Guid.NewGuid();
        //     Guid fileStorageId = Guid.NewGuid();
        //     MimeType mimeType = MimeType.Jpg;
        //     var manufacturer = GetManufacturer();
        //
        //     //act
        //     Action action = () => manufacturer.AttachPicture(pictureId,
        //         fileStorageId,
        //         filename,
        //         description,
        //         url,
        //         mimeType);
        //
        //     //arrange
        //     action.Should().Throw<DomainException>();
        // }
        //
        //
        // [Fact]
        // public void Should_DeletePicture_DeletedFromPictures()
        // {
        //     //arrange
        //     Guid pictureId = Guid.NewGuid();
        //     Guid fileStorageId = Guid.NewGuid();
        //     string filename = "seoFilename";
        //     string description = "pictureDescription";
        //     string url = "https://ubiquitous.com/product/picture/123923";
        //     MimeType mimeType = MimeType.Jpg;
        //     var product = GetManufacturer();
        //
        //     //act
        //     product.AttachPicture(pictureId,
        //         fileStorageId,
        //         filename,
        //         description,
        //         url,
        //         mimeType);
        //
        //     product.DetachPicture(pictureId);
        //
        //     //arrange
        //     var picture = product.Pictures.FirstOrDefault();
        //     picture.Should().BeNull();
        // }
        //
        // [Fact]
        // public void Should_TryDeletePictureNotExistingPicture_ThrownDomainException()
        // {
        //     //arrange
        //     Guid pictureId = Guid.NewGuid();
        //     Guid fileStorageId = Guid.NewGuid();
        //     string filename = "seoFilename";
        //     string description = "pictureDescription";
        //     string url = "https://ubiquitous.com/product/picture/123923";
        //     MimeType mimeType = MimeType.Jpg;
        //     var manufacturer = GetManufacturer();
        //
        //     //act
        //     manufacturer.AttachPicture(pictureId,
        //         fileStorageId,
        //         filename,
        //         description,
        //         url,
        //         mimeType);
        //
        //     Action action = () => manufacturer.DetachPicture(Guid.Empty);
        //
        //     action.Should().Throw<DomainException>();
        // }

        [Fact]
        public void Should_GetDraftManufacturer_DefinedManufacturer()
        {
            //arrange
            //act
            var manufacturer = Domain.Entities.Manufacturer.Manufacturer.GetDraftManufacturer();

            //assert
            manufacturer.Should().NotBeNull();
        }

        private Domain.Entities.Manufacturer.Manufacturer GetManufacturer()
        {
            return new Domain.Entities.Manufacturer.Manufacturer(_manufacturerId,
                _uniqueClientId,
                _name,
                _description);
        }
    }
}