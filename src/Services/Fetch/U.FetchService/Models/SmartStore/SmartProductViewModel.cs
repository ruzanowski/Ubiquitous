using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using U.FetchService.Models.SmartStore;

namespace U.FetchService.Commands.UpdateProducts.ViewModel
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