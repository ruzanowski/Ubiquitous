using System;
using System.Linq;
using FluentAssertions;
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

        [Fact]
        public void Should_AddPicture_AddedPictureAndRaisedPictureAddedDomainEvent()
        {
            //arrange
            Guid pictureId = Guid.NewGuid();

            var manufacturer = GetManufacturer();
            var lastUpdatedAt = manufacturer.LastUpdatedAt;
            var lastUpdatedBy = manufacturer.LastUpdatedBy;
            var createdAt = manufacturer.CreatedAt;
            var createdBy = manufacturer.CreatedBy;

            //act
            manufacturer.AttachPicture(pictureId);

            //arrange
            manufacturer.AggregateId.Should().Be(_manufacturerId);
            manufacturer.Id.Should().Be(_manufacturerId);
            manufacturer.Name.Should().Be(_name);
            manufacturer.Description.Should().Be(_description);
            manufacturer.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            manufacturer.LastUpdatedBy.Should().Be(lastUpdatedBy);
            manufacturer.CreatedAt.Should().Be(createdAt);
            manufacturer.CreatedBy.Should().Be(createdBy);
            manufacturer.AggregateTypeName.Should().Be(nameof(Domain.Entities.Manufacturer.Manufacturer));
            manufacturer.UniqueClientId.Should().Be(_uniqueClientId);
            var picture = manufacturer.Pictures.First();
            picture.PictureId.Should().Be(pictureId);
            picture.ManufacturerId.Should().Be(manufacturer.Id);
        }


        [Fact]
        public void Should_TryDeletePictureNotExistingPicture_ThrownDomainException()
        {
            //arrange
            Guid pictureId = Guid.NewGuid();
            var manufacturer = GetManufacturer();

            //act
            manufacturer.AttachPicture(pictureId);

            Action action = () => manufacturer.DetachPicture(Guid.Empty);

            action.Should().Throw<DomainException>();
        }

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