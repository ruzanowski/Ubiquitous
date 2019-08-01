using System.Threading.Tasks;
using RestEase;
using U.Common.Pagination;
using U.FetchService.Commands.UpdateProducts.ViewModel;

namespace U.FetchService.Services
{
    public interface ISmartStoreAdapter
    {
        [AllowAnyStatusCode]
        [Get("api/smartstore/products/get-list")]
        Task<PaginatedItems<SmartProductViewModel>> GetListAsync(
            [Query("pageSize")] int pageSize = 99999,
            [Query("pageIndex")] int pageIndex = 0);
    }
}