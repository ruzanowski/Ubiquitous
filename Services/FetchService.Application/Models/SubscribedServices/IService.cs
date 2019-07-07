using System.Collections.Generic;
using System.Threading.Tasks;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Models.SubscribedServices
{
    public interface IService
    {
        PartySettings Settings { get; set; }
        Task<bool> ForwardData(IEnumerable<SmartProductViewModel> products);
    }
}