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
}