using System.Threading.Tasks;
using U.Common;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Models.Wholesales
{
    public interface IWholesale
    {
        PartySettings Settings { get; set; }
        Task<PaginatedItems<SmartProductViewModel>> FetchProducts();
    }    
}