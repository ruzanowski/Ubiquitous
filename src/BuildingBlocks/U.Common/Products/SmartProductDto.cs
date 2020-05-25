using System.Diagnostics.CodeAnalysis;

namespace U.Common.Products
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SmartProductDto
    {
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public string BarCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int CategoryId { get; set; }
    }
}