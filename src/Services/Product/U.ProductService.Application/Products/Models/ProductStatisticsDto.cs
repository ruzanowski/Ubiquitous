using System;
using Newtonsoft.Json;

namespace U.ProductService.Application.Products.Models
{
    public class ProductStatisticsDto
    {
        public DateTime DateTime { get; set; }
        public int Count { get; set; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? CategoryId { get; set; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Description { get; set; }

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