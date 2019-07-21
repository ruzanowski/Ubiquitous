using System;
using System.Collections.Generic;
using U.ProductService.Domain.Events;
using U.ProductService.Domain.SeedWork;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace U.ProductService.Domain.Aggregates.Product
{
    public class Product : Entity, IAggregateRoot
    {

        public string UniqueProductCode { get; private set; }
        
        public string Name { get; private set; }
        public string ProductUniqueCode { get; private set; }
        public string ManufacturerPartNumber { get; private set; }
        public int InStock { get; private set; }
        public int TaxCategoryId { get; private set; }
        public decimal PriceInTax { get; private set; }
        public decimal ProductCost { get; private set; }
        public decimal PriceMinimumSpecifiedByCustomer { get; private set; }
        public string Description { get; private set; }
        public string CountryMade { get; private set; }
        public bool IsPublished { get; private set; }
        
        //ValueObjects
        public Address Address { get; private set; }
        public Dimensions Dimensions { get; private set; }
        
        public Guid ManufacturerId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? MainPictureId { get; private set; }
        public IEnumerable<Guid> PicturesIds { get; private set; }

        protected Product()
        {
            Id = Guid.NewGuid();
        }

        public Product(Guid manufacturerId, Address address) : this()
        {
            ManufacturerId = manufacturerId; 
            Address = address;
            
            var productAddedEvent = new ProductAddedDomainEvent(Id, manufacturerId);        
            
            this.AddDomainEvent(productAddedEvent);
        }

    }
}

