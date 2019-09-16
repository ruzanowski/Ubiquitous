using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace U.SmartStoreAdapter.Application.Models.Products
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SmartProductDto
    {
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
        public bool IsAvailable { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int? MainPictureId { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<int> ProductTags { get; set; }
        public IEnumerable<int> PicturesIds { get; set; }
        public string UrlSlug { get; set; }
        
        
    }
}