using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace U.FetchService.Models.SmartStore
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SmartProductViewModel : SmartProductDto
    {
        public string Id { get; set; }
        public string CountryMade { get; set; }
        [JsonIgnore]
        public new bool IsAvailable { get; set; }
        public bool IsPublished { get; set; }
    }
}