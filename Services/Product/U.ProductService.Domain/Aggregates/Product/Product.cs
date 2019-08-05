using System;
using System.Collections.Generic;
using U.ProductService.Domain.Events;
using U.ProductService.Domain.SeedWork;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable CheckNamespace


namespace U.ProductService.Domain.Aggregates
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string BarCode { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? LastFullUpdateDateTime { get; private set; }

        //value object
        public Dimensions Dimensions { get; private set; }

        //manufacturer
        public Guid ManufacturerId { get; private set; }

        //pictures
        private readonly List<Picture> _pictures;
        public IReadOnlyCollection<Picture> Pictures => _pictures;

        private Product()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            BarCode = string.Empty;
            Description = string.Empty;
            IsPublished = default;
            CreatedDateTime = DateTime.UtcNow;
            LastFullUpdateDateTime = default; 
            IsPublished = default;
        }

        public Product(string name, decimal price, string barCode, string description, Dimensions dimensions, Guid manufacturerId) : this()
        {
            Name = name;
            Price = price;
            BarCode = barCode;
            Description = description;
            Dimensions = dimensions;
            ManufacturerId = manufacturerId;

            var @event = new ProductAddedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public bool CompareAlternateId(string productUniqueCode) => BarCode.Equals(productUniqueCode);

        public void AddPicture(string seoFilename, string description, string url, string mimeType)
        {
            _pictures.Add(new Picture(seoFilename, description, url, mimeType));

            var @event = new ProductPictureAddedDomainEvent(Id, seoFilename);
            
            AddDomainEvent(@event);
        }
        
        public void DeletePicture(Guid pictureId)
        {
            _pictures.Remove(_pictures.Find(x => x.Id.Equals(pictureId)));
            
            var @event = new ProductPictureRemovedDomainEvent(Id);
            
            AddDomainEvent(@event);
        }

        public void ChangePrice(decimal price)
        {
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

        public void UpdateAllProperties(string name, decimal price, Dimensions dimensions, DateTime updateGenerated)
        {
            if (LastFullUpdateDateTime < updateGenerated)
            {
                //avoiding any lagged-in-time updates 
                Name = name;
                Price = price;
                Dimensions.Height = dimensions.Height;
                Dimensions.Length = dimensions.Length;
                Dimensions.Weight = dimensions.Weight;
                Dimensions.Width = dimensions.Width;
                
                //add new update event
            }
            
            // add new update saying event has been raised after last up-to-date update
        }
    }
}