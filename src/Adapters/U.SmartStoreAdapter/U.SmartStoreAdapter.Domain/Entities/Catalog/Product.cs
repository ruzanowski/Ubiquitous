using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public int StockQuantity{ get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int? ManufacturerId { get; set; }

    }
}