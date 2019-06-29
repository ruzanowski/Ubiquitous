using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace U.SmartStoreAdapter.Api.Products
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SmartProductViewModel : SmartProductDto
    {
        public string Id { get; set; } // customerId.productId
        public string CountryMade { get; set; }
        [JsonIgnore]
        public new bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
    }
}