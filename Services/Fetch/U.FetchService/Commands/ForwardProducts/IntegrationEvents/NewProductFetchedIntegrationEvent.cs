using System.Collections.Generic;
using U.EventBus.Events;

namespace U.FetchService.Commands.ForwardProducts.IntegrationEvents
{
    public class NewProductFetchedIntegrationEvent : IntegrationEvent
    {
        public string Name { get; private set; }
        public int ManufacturerId { get; private set; }
        public string ProductUniqueCode { get; private set; }
        public string ManufacturerPartNumber { get; private set; }
        public int InStock { get; private set; }
        public int TaxCategoryId { get; private set; }
        public decimal PriceInTax { get; private set; }
        public decimal ProductCost { get; private set; }
        public decimal PriceMinimumSpecifiedByCustomer { get; private set; }
        public string Description { get; private set; }
        public decimal Length { get; private set; }
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Weight { get; private set; }
        public int? MainPictureId { get; private set; }
        public int CategoryId { get; private set; }
        public IEnumerable<int> ProductTags { get; private set; }
        public IEnumerable<int> PicturesIds { get; private set; }
        public string UrlSlug { get; private set; }
        public string Id { get; private set; } // customerId.productId
        public string CountryMade { get; private set; }
        public bool IsPublished { get; private set; }

        public NewProductFetchedIntegrationEvent(string name, int manufacturerId, string productUniqueCode,
            string manufacturerPartNumber, int inStock, int taxCategoryId, decimal priceInTax, decimal productCost,
            decimal priceMinimumSpecifiedByCustomer, string description, decimal length, decimal width, decimal height,
            decimal weight, int? mainPictureId, int categoryId, IEnumerable<int> productTags,
            IEnumerable<int> picturesIds, string urlSlug, string id, string countryMade, bool isPublished)
        {
            Name = name;
            ManufacturerId = manufacturerId;
            ProductUniqueCode = productUniqueCode;
            ManufacturerPartNumber = manufacturerPartNumber;
            InStock = inStock;
            TaxCategoryId = taxCategoryId;
            PriceInTax = priceInTax;
            ProductCost = productCost;
            PriceMinimumSpecifiedByCustomer = priceMinimumSpecifiedByCustomer;
            Description = description;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            MainPictureId = mainPictureId;
            CategoryId = categoryId;
            ProductTags = productTags;
            PicturesIds = picturesIds;
            UrlSlug = urlSlug;
            Id = id;
            CountryMade = countryMade;
            IsPublished = isPublished;
        }
    }
}