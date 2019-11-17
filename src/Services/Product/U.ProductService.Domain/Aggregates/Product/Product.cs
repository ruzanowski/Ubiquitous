using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Domain.Aggregates.Picture;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Events;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.Helpers;
using U.ProductService.Domain.SeedWork;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public class Product : Entity, IAggregateRoot, ITrackable, IPublishable, IPictureManagable
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
        public ICollection<Picture> Pictures { get; private set; }
        public Guid CategoryId { get; private set; }
        public virtual Category Category { get; private set; }
        public int ProductTypeId { get; private set; }
        public ProductType ProductType { get; private set; }

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
        }

        public Product(string name, decimal price, string barCode, string description, Dimensions dimensions,
            Guid manufacturerId, Guid categoryId, int productTypeId) : this()
        {
            Name = name;
            Price = price;
            BarCode = barCode;
            Description = description;
            Dimensions = dimensions;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
            ProductTypeId = productTypeId;

            var @event = new ProductAddedDomainEvent(Id, Name, Price, ManufacturerId, categoryId);

            AddDomainEvent(@event);
        }


        public void AddPicture(Guid id, Guid fileStorageUploadId, string seoFilename, string description, string url,
            MimeType mimeType)
        {
            if (string.IsNullOrEmpty(seoFilename))
                throw new ProductDomainException($"{nameof(seoFilename)} cannot be null or empty!");

            if (string.IsNullOrEmpty(description))
                throw new ProductDomainException($"{nameof(description)} cannot be null or empty!");

            if (string.IsNullOrEmpty(url))
                throw new ProductDomainException($"{nameof(url)} cannot be null or empty!");

            var picture = new Picture(id, Id, nameof(Product), fileStorageUploadId, seoFilename, description, url,
                mimeType);

            Pictures.Add(picture);

            var @event = new ProductPictureAddedDomainEvent(Id, picture.Id, seoFilename);

            AddDomainEvent(@event);
        }

        public void DeletePicture(Guid pictureId)
        {
            var picture = Pictures.FirstOrDefault(x => x.Id.Equals(pictureId));

            if (picture is null)
                throw new ProductDomainException("Picture does not exist!");

            Pictures.Remove(picture);

            var @event = new ProductPictureRemovedDomainEvent(Id, picture.Id);

            AddDomainEvent(@event);
        }

        public void ChangePrice(decimal price)
        {
            if (price < 0)
                throw new ProductDomainException("Price cannot be below 0!");

            var previousPrice = Price;

            Price = price;

            var @event = new ProductPriceChangedDomainEvent(Id, previousPrice, Price);

            AddDomainEvent(@event);
        }

        public void Publish()
        {
            if (IsPublished) return;
            IsPublished = true;

            var @event = new ProductPublishedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public void UnPublish()
        {
            if (IsPublished == false) return;

            IsPublished = false;

            var @event = new ProductUnpublishedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public void UpdateProduct(IMapper mapper, string name, string description, decimal price, Dimensions dimensions,
            DateTime updateDispatchedFromOrigin)
        {
            if (!LastUpdatedAt.HasValue || LastUpdatedAt.Value >= updateDispatchedFromOrigin) return;

            var deepCopyProduct = mapper.Map<Product>(this);
            var variances = this.ExamineProductVariances(deepCopyProduct);
            if (variances.Any())
            {
                UpdateProperties(this, name, description, price, dimensions);

                var @event = new ProductPropertiesChangedDomainEvent(Id, ManufacturerId, variances);
                AddDomainEvent(@event);

                // add new update saying event has been raised after last up-to-date update
            }
        }


        private void UpdateProperties(Product product, string name, string description, decimal price,
            Dimensions dimensions)
        {
            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Dimensions.Height = dimensions.Height;
            product.Dimensions.Length = dimensions.Length;
            product.Dimensions.Weight = dimensions.Weight;
            product.Dimensions.Width = dimensions.Width;
        }


        public Product UpdatedDeepCopy(IMapper mapper, string name, string description, decimal price,
            Dimensions dimensions)
        {
            var deepCopyProduct = mapper.Map<Product>(this);
            UpdateProperties(deepCopyProduct, name, description, price, dimensions);
            return deepCopyProduct;
        }

        public void ChangeCategory(Guid newCategoryId)
        {
            CategoryId = newCategoryId;
        }
    }
}