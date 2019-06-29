using System.Threading.Tasks;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Models
{
    public interface IWholesale
    {
        Task<PaginatedItems<SmartProductViewModel>> FetchProducts();
    }    
}