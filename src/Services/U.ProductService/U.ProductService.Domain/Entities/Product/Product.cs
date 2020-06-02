using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Picture;
using U.ProductService.Domain.Entities.Product.Events;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Entities.Product
{
    public sealed class Product : Entity, IAggregateRoot, ITrackable, IPublishable, IEquatable<Product>
    {
        public Guid AggregateId => Id;
        public string AggregateTypeName => nameof(Product);
        public string Name { get; private set; }
        public string BarCode { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public bool IsPublished { get; private set; }

        private DateTime _createdAt;
        private string _createdBy;
        private DateTime? _lastUpdatedAt;
        private string _lastUpdatedBy;
        public DateTime CreatedAt => _createdAt;
        public string CreatedBy => _createdBy;
        public DateTime? LastUpdatedAt => _lastUpdatedAt;
        public string LastUpdatedBy => _lastUpdatedBy;

        public Dimensions Dimensions { get; private set; }
        public Guid ManufacturerId { get; private set; }
        public ICollection<ProductPicture> Pictures { get; private set; }
        public Guid CategoryId { get; private set; }
        public ProductCategory ProductCategory { get; private set; }
        public int ProductTypeId { get; private set; }
        public ProductType ProductType { get; private set; }
        public string ExternalSourceName { get; private set; }
        public string ExternalId { get; private set; }

        [JsonConstructor]
        private Product()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            BarCode = string.Empty;
            Description = string.Empty;
            _createdAt = DateTime.UtcNow;
            _createdBy = string.Empty;
            _lastUpdatedAt = default;
            _lastUpdatedBy = string.Empty;
            IsPublished = false;
            Pictures = new HashSet<ProductPicture>();
        }

        public Product(string name,
            decimal price,
            string barCode,
            string description,
            Dimensions dimensions,
            Guid manufacturerId,
            Guid categoryId,
            int productTypeId,
            string externalSourceName,
            string externalId) : this()
        {
            Name = name;
            Price = price;
            BarCode = barCode;
            Description = description;
            Dimensions = dimensions;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
            ProductTypeId = productTypeId;
            ExternalSourceName = externalSourceName;
            ExternalId = externalId;

            var @event = new ProductAddedDomainEvent(
                Id,
                Name,
                Price,
                ManufacturerId,
                categoryId,
                externalSourceName,
                externalId);

            AddDomainEvent(@event);
        }


        public void AttachPicture(Guid pictureId)
        {
            var picture = new ProductPicture
            {
                ProductId = Id,
                PictureId = pictureId
            };

            Pictures.Add(picture);

            var @event = new ProductPictureAddedDomainEvent(Id, picture.PictureId);

            AddDomainEvent(@event);
        }

        public void ChangePrice(decimal price)
        {
            if (price < 0)
                throw new DomainException("Price must not be below 0!");

            if (price == Price)
                return;

            var previousPrice = Price;

            Price = price;

            var @event = new ProductPriceChangedDomainEvent(Id, previousPrice, Price);

            AddDomainEvent(@event);
        }

        public void ChangeCategory(Guid newCategoryId)
        {
            CategoryId = newCategoryId;
        }

        public void DetachPicture(Guid pictureId)
        {
            var picture = Pictures.FirstOrDefault(x => x.PictureId.Equals(pictureId));

            if (picture is null)
                throw new NotFoundDomainException("Picture does not exist!");

            Pictures.Remove(picture);

            var @event = new ProductPictureRemovedDomainEvent(Id, picture.PictureId);

            AddDomainEvent(@event);
        }

        public void Unpublish()
        {
            if (IsPublished == false) return;

            IsPublished = false;

            var @event = new ProductUnpublishedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public bool UpdateProperties(string name,
            string description,
            decimal price,
            Dimensions dimensions,
            DateTime updateDispatchedFromOrigin)
        {
            //if last update is fresher than intended change queued,
            //then such action may invalid newest state
            if ((LastUpdatedAt ?? DateTime.MinValue) >= updateDispatchedFromOrigin)
                return false;

            var isAllEqual = AllEqual(name, description, price, dimensions);

            if (isAllEqual)
                return false;

            UpdateBasicProperties(name, description, price, dimensions);

            var @event = new ProductPropertiesChangedDomainEvent(Id, ManufacturerId, name, price, description, dimensions);
            AddDomainEvent(@event);
            return true;
        }

        public void Publish()
        {
            if (IsPublished) return;
            IsPublished = true;

            var @event = new ProductPublishedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        private bool AllEqual(string name,
            string description,
            decimal price,
            Dimensions dimensions) =>
            Name.Equals(name) &&
            Description.Equals(description) &&
            Price.Equals(price) &&
            Dimensions.Height.Equals(dimensions.Height) &&
            Dimensions.Weight.Equals(dimensions.Weight) &&
            Dimensions.Width.Equals(dimensions.Width) &&
            Dimensions.Length.Equals(dimensions.Length);

        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other)
                   && string.Equals(Name, other.Name)
                   && string.Equals(BarCode, other.BarCode)
                   && Price == other.Price
                   && string.Equals(Description, other.Description)
                   && IsPublished == other.IsPublished
                   && Equals(Dimensions, other.Dimensions)
                   && ManufacturerId.Equals(other.ManufacturerId)
                   && Equals(Pictures, other.Pictures)
                   && CategoryId.Equals(other.CategoryId)
                   && Equals(ProductCategory, other.ProductCategory)
                   && ProductTypeId == other.ProductTypeId
                   && Equals(ProductType, other.ProductType)
                   && string.Equals(ExternalSourceName, other.ExternalSourceName)
                   && string.Equals(ExternalId, other.ExternalId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Product) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (BarCode != null ? BarCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsPublished.GetHashCode();
                hashCode = (hashCode * 397) ^ (Dimensions != null ? Dimensions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ManufacturerId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Pictures != null ? Pictures.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CategoryId.GetHashCode();
                hashCode = (hashCode * 397) ^ (ProductCategory != null ? ProductCategory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ProductTypeId;
                hashCode = (hashCode * 397) ^ (ProductType != null ? ProductType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExternalSourceName != null ? ExternalSourceName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExternalId != null ? ExternalId.GetHashCode() : 0);
                return hashCode;
            }
        }

        private void UpdateBasicProperties(string name,
            string description,
            decimal price,
            Dimensions dimensions)
        {
            Name = name;
            Description = description;
            Price = price;
            Dimensions = dimensions;
        }
    }
}