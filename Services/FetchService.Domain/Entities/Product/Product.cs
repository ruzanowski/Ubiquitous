using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using U.FetchService.Domain.Entities.Common;
using U.FetchService.Domain.Entities.ProductTags;

namespace U.FetchService.Domain.Entities.Product
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Product
    {
        public Product()
        {
            ProductTags = new HashSet<ProductTag>();
            Pictures = new HashSet<Picture.Picture>();
        }
        
        public string Id { get; set; } // customerId.productId
        public string CountryMade { get; set; }
        public bool IsPublished { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public string ProductUniqueCode { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public int InStock { get; set; }
        public int TaxCategoryId { get; set; }
        public decimal PriceInTax { get; set; }
        public decimal ProductCost { get; set; }
        public decimal PriceMinimumSpecifiedByCustomer { get; set; }
        public string Description { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int? MainPictureId { get; set; }
        public int CategoryId { get; set; }
        public string UrlSlug { get; set; }
        public Modified Created { get; set; }
        public Modified LastModified { get; set; }
        public IEnumerable<ProductTag> ProductTags { get; private set; }
        public IEnumerable<Picture.Picture> Pictures { get; private set; }

        public void SetProductTags(IEnumerable<ProductTag> productTags)
        {
            ProductTags = productTags;
        }
        
        public void SetPictures(IEnumerable<Picture.Picture> pictures)
        {
            Pictures = pictures;
        }
    }
}