using System;

namespace U.ProductService.Application.Products.Models
{
    public class ProductStatisticsDto
    {
        public DateTime DateTime { get; set; }
        public int Count { get; set; }

    }

    public class ProductByManufacturersStatisticsDto
    {
        public int Count { get; set; }
        public string ManufacturerName { get; set; }
    }

    public class ProductByCategoryStatisticsDto
    {
        public int Count { get; set; }
        public string CategoryName { get; set; }
    }
}