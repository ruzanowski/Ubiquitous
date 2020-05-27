using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Picture;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Domain.Aggregates.Product.Events;
using U.ProductService.Domain.Exceptions;
using Xunit;

namespace U.ProductService.DomainTests.Product
{
    public class ProductTest
    {
        private readonly string _name = "eventName";
        private readonly decimal _price = 999;
        private readonly string _barCode = "randomBarCode";
        private readonly string _description = "description";
        private readonly Dimensions _dimensions = new Dimensions(1, 2, 3, 4);
        private readonly Guid _manufacturerId = Guid.NewGuid();
        private readonly Guid _categoryId = Guid.NewGuid();
        private readonly int _productTypeId = ProductType.SimpleProduct.Id;
        private readonly string _externalSourceName = "externalSourceName";
        private readonly string _externalId = Guid.NewGuid().ToString();

        [Fact]
        public void Should_OnCreation_CorrectlyAssignArgumentsAndRaiseProductAddedDomainEvent()
        {
            //arrange
            //act
            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;

            var productAddedDomainEvent = new ProductAddedDomainEvent(
                product.Id,
                _name,
                _price,
                _manufacturerId,
                _categoryId,
                _externalSourceName,
                _externalId);

            var domainEvent = JsonConvert.SerializeObject(product.DomainEvents.First());
            var expectedDomainEvent = JsonConvert.SerializeObject(productAddedDomainEvent);

            //assert
            product.AggregateId.Should().Be(productAddedDomainEvent.ProductId);
            product.Id.Should().Be(productAddedDomainEvent.ProductId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
            expectedDomainEvent.Should().Be(domainEvent);
        }

        [Fact]
        public void Should_AddPicture_AddedPictureAndRaisedPictureAddedDomainEvent()
        {
            //arrange
            Guid pictureId = Guid.NewGuid();
            Guid fileStorageId = Guid.NewGuid();
            string filename = "seoFilename";
            string description = "pictureDescription";
            string url = "https://ubiquitous.com/product/picture/123923";
            MimeType mimeType = MimeType.Jpg;

            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;

            //act
            product.AddPicture(pictureId,
                fileStorageId,
                filename,
                description,
                url,
                mimeType);

            var pictureEvent = product.DomainEvents.ElementAt(1) as ProductPictureAddedDomainEvent;
            var domainEvent = JsonConvert.SerializeObject(pictureEvent);
            var expectedDomainEvent =
                JsonConvert.SerializeObject(new ProductPictureAddedDomainEvent(product.Id, pictureId));

            //arrange

            product.AggregateId.Should().Be(pictureEvent!.ProductId);
            product.Id.Should().Be(pictureEvent!.ProductId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            var picture = product.Pictures.First();
            picture.Id.Should().Be(pictureId);
            picture.Url.Should().Be(url);
            picture.Description.Should().Be(description);
            picture.FileName.Should().Be(filename);
            picture.MimeType.Should().Be(mimeType);
            picture.FileStorageUploadId.Should().Be(fileStorageId);
            picture.AggregateRootId.Should().Be(product.Id);
            picture.AggregateRootName.Should().Be(nameof(Domain.Aggregates.Product.Product));
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
            expectedDomainEvent.Should().Be(domainEvent);
        }

        [Theory]
        [InlineData("", "pictureDescription", "https://ubiquitous.com/product/picture/123923")]
        [InlineData(null, "pictureDescription", "https://ubiquitous.com/product/picture/123923")]
        [InlineData("fileName", "", "https://ubiquitous.com/product/picture/123923")]
        [InlineData("fileName", null, "https://ubiquitous.com/product/picture/123923")]
        [InlineData("fileName", "pictureDescription", "")]
        [InlineData("fileName", "pictureDescription", null)]
        public void Should_AddPictureWithNullArguments_ThrownDomainException(string filename, string description, string url)
        {
            //arrange
            Guid pictureId = Guid.NewGuid();
            Guid fileStorageId = Guid.NewGuid();
            MimeType mimeType = MimeType.Jpg;
            var product = GetProduct();

            //act
            Action action = () => product.AddPicture(pictureId,
                fileStorageId,
                filename,
                description,
                url,
                mimeType);

            //arrange
            action.Should().Throw<DomainException>();
        }

        [Fact]
        public void Should_DeletePicture_DeletedFromPicturesAndRaisedRemovedDomainEvent()
        {
            //arrange
            Guid pictureId = Guid.NewGuid();
            Guid fileStorageId = Guid.NewGuid();
            string filename = "seoFilename";
            string description = "pictureDescription";
            string url = "https://ubiquitous.com/product/picture/123923";
            MimeType mimeType = MimeType.Jpg;
            var product = GetProduct();

            //act
            product.AddPicture(pictureId,
                fileStorageId,
                filename,
                description,
                url,
                mimeType);

            product.DeletePicture(pictureId);

            var domainEvent = JsonConvert.SerializeObject(product.DomainEvents.ElementAt(2));
            var expectedDomainEvent =
                JsonConvert.SerializeObject(new ProductPictureRemovedDomainEvent(product.Id, pictureId));

            //arrange
            var picture = product.Pictures.FirstOrDefault();
            picture.Should().BeNull();
            expectedDomainEvent.Should().Be(domainEvent);
        }

        [Fact]
        public void Should_TryDeletePictureNotExistingPicture_ThrownDomainException()
        {
            //arrange
            Guid pictureId = Guid.NewGuid();
            Guid fileStorageId = Guid.NewGuid();
            string filename = "seoFilename";
            string description = "pictureDescription";
            string url = "https://ubiquitous.com/product/picture/123923";
            MimeType mimeType = MimeType.Jpg;
            var product = GetProduct();

            //act
            product.AddPicture(pictureId,
                fileStorageId,
                filename,
                description,
                url,
                mimeType);

            Action action = () => product.DeletePicture(Guid.Empty);

            action.Should().Throw<DomainException>();
        }

        [Fact]
        public void Should_ChangePrice_PriceChangedAndRaisedProductPriceChanged()
        {
            //arrange
            var product = GetProduct();
            decimal oldPrice = _price;
            decimal newPrice = _price + 100;
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;

            //act
            product.ChangePrice(newPrice);

            var productAddedDomainEvent = new ProductPriceChangedDomainEvent(product.Id,
                oldPrice,
                newPrice);
            var domainEvent = JsonConvert.SerializeObject(product.DomainEvents.ElementAt(1));
            var expectedDomainEvent = JsonConvert.SerializeObject(productAddedDomainEvent);

            //assert
            product.AggregateId.Should().Be(productAddedDomainEvent.ProductId);
            product.Id.Should().Be(productAddedDomainEvent.ProductId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(newPrice);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
            expectedDomainEvent.Should().Be(domainEvent);
        }


        [Fact]
        public void Should_ChangePriceBelowZero_ThrownDomainException()
        {
            //arrange
            var product = GetProduct();
            decimal newPrice = -1;

            //act
            Action action = () => product.ChangePrice(newPrice);

            //assert
            action.Should().Throw<DomainException>();
        }

        [Fact]
        public void Should_Publish_PublishedAndRaisedProductPublishedDomainEvent()
        {
            //arrange
            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;

            //act
            product.Publish();

            var productAddedDomainEvent = new ProductPublishedDomainEvent(product.Id,
                product.Name,
                product.Price,
                product.ManufacturerId);
            var domainEvent = JsonConvert.SerializeObject(product.DomainEvents.ElementAt(1));
            var expectedDomainEvent = JsonConvert.SerializeObject(productAddedDomainEvent);

            //assert
            product.AggregateId.Should().Be(productAddedDomainEvent.ProductId);
            product.Id.Should().Be(productAddedDomainEvent.ProductId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeTrue();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            expectedDomainEvent.Should().Be(domainEvent);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
        }

        [Fact]
        public void Should_UnPublish_UnpublishedAndRaisedProductPublishedDomainEvent()
        {
            //arrange
            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;

            //act
            product.Publish();
            product.UnPublish();

            var productAddedDomainEvent = new ProductUnpublishedDomainEvent(
                product.Id,
                product.Name,
                product.Price,
                product.ManufacturerId);

            var domainEvent = JsonConvert.SerializeObject(product.DomainEvents.ElementAt(2));
            var expectedDomainEvent = JsonConvert.SerializeObject(productAddedDomainEvent);

            //assert
            product.AggregateId.Should().Be(productAddedDomainEvent.ProductId);
            product.Id.Should().Be(productAddedDomainEvent.ProductId);
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            expectedDomainEvent.Should().Be(domainEvent);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
        }

        [Fact]
        public void Should_UpdateProperties_UpdatedPropertiesAndRaisedProductPropertiesChangedDomainEvent()
        {
            //arrange
            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;

            string newName = "newName";
            string newDescription = "newDescription";
            decimal newPrice = _price + 100;
            Dimensions newDimensions = _dimensions;
            newDimensions.Height = _dimensions.Height + 5;
            newDimensions.Weight = _dimensions.Weight + 5;
            newDimensions.Length = _dimensions.Length + 5;
            newDimensions.Width = _dimensions.Width + 5;
            DateTime dateTimeSent = DateTime.UtcNow;

            //act
            var isUpdated = product.UpdateProperties(newName,
                newDescription,
                newPrice,
                newDimensions,
                dateTimeSent);

            var productAddedDomainEvent = new ProductPropertiesChangedDomainEvent(
                product.Id,
                product.ManufacturerId,
                product.Name,
                product.Price,
                product.Description,
                product.Dimensions);

            var domainEvent = JsonConvert.SerializeObject(product.DomainEvents.ElementAt(1));
            var expectedDomainEvent = JsonConvert.SerializeObject(productAddedDomainEvent);

            //assert
            isUpdated.Should().BeTrue();
            product.AggregateId.Should().Be(productAddedDomainEvent.ProductId);
            product.Id.Should().Be(productAddedDomainEvent.ProductId);
            product.Name.Should().Be(newName);
            product.Description.Should().Be(newDescription);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(newPrice);
            product.Dimensions.Should().Be(newDimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.Category.Should().BeNull();
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            expectedDomainEvent.Should().Be(domainEvent);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));

        }

        [Fact]
        public void Should_TryUpdatePropertiesWithSameParameters_ResultsNoChange()
        {
            //arrange
            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;
            DateTime dateTimeSent = DateTime.MinValue;

            //act
            var isUpdated = product.UpdateProperties(_name,
                _description,
                _price,
                _dimensions,
                dateTimeSent);

            //assert
            isUpdated.Should().BeFalse();
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.DomainEvents.Count.Should().Be(1);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
        }

        [Fact]
        public void Should_ChangeCategory_ChangedCategory()
        {
            //arrange
            var product = GetProduct();
            var lastUpdatedAt = product.LastUpdatedAt;
            var lastUpdatedBy = product.LastUpdatedBy;
            var createdAt = product.CreatedAt;
            var createdBy = product.CreatedBy;
            Guid newCategoryId = Guid.NewGuid();

            //act
            product.ChangeCategory(newCategoryId);

            //assert
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(newCategoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.DomainEvents.Count.Should().Be(1);
            product.LastUpdatedAt.Should().NotBe(lastUpdatedAt ?? DateTime.UtcNow);
            product.LastUpdatedBy.Should().Be(lastUpdatedBy);
            product.CreatedAt.Should().Be(createdAt);
            product.CreatedBy.Should().Be(createdBy);
            product.AggregateTypeName.Should().Be(nameof(Domain.Aggregates.Product.Product));
        }

        [Fact]
        public void Should_TwoSameProductEquals_Equals()
        {
            //arrange
            var product = GetProduct();
            var product2 = product;

            //act
            var isEqual = product.Equals(product2);
            var isEqual2 = product.Equals((object) product);
            var hashcode = product.GetHashCode();
            var hashcode2 = product2.GetHashCode();

            //assert
            isEqual.Should().BeTrue();
            isEqual2.Should().BeTrue();
            hashcode.Should().Be(hashcode2);

        }

        [Fact]
        public void Should_RemoveDomainEvent_RemovedDomainEventAndDomainEventRemainsEmpty()
        {
            //arrange
            var product = GetProduct();
            var domainEvent = product.DomainEvents.First();

            //act
            product.RemoveDomainEvent(domainEvent);

            //arrange
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.DomainEvents.Count.Should().Be(0);
        }

        [Fact]
        public void Should_ClearDomainEvents_NoDomainEventsLeft()
        {
            //arrange
            var product = GetProduct();

            //act
            product.ClearDomainEvents();

            //arrange
            product.Name.Should().Be(_name);
            product.Description.Should().Be(_description);
            product.BarCode.Should().Be(_barCode);
            product.Price.Should().Be(_price);
            product.Dimensions.Should().Be(_dimensions);
            product.ManufacturerId.Should().Be(_manufacturerId);
            product.CategoryId.Should().Be(_categoryId);
            product.ExternalSourceName.Should().Be(_externalSourceName);
            product.ExternalId.Should().Be(_externalId);
            product.IsPublished.Should().BeFalse();
            product.ProductType.Should().Be(ProductType.SimpleProduct);
            product.ProductTypeId.Should().Be(ProductType.SimpleProduct.Id);
            product.DomainEvents.Count.Should().Be(0);
        }

        private Domain.Aggregates.Product.Product GetProduct()
        {
            return new Domain.Aggregates.Product.Product(_name,
                _price,
                _barCode,
                _description,
                _dimensions,
                _manufacturerId,
                _categoryId,
                _productTypeId,
                _externalSourceName,
                _externalId);
        }
    }
}