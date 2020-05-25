using System.Threading.Tasks;
using RestEase;
using U.Common;
using U.Common.Pagination;
using U.Common.Products;

namespace U.FetchService.Services
{
    public interface ISmartStoreAdapter
    {
        [AllowAnyStatusCode]
        [Get(GlobalConstants.SmartStoreQueryProductsPath)]
        Task<PaginatedItems<SmartProductViewModel>> GetProductsAsync(
            [Query("pageSize")] int pageSize = int.MaxValue,
            [Query("pageIndex")] int pageIndex = 0);
    }
}