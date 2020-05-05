using MediatR;
using U.Common.Pagination;
using U.SmartStoreAdapter.Application.Common.QueryModels;

namespace U.SmartStoreAdapter.Application.Products
{
    public class GetProductsListQuery :  IRequest<PaginatedItems<SmartProductViewModel>>, IPagination
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
        public PriceWindow Price { get; set; }
        public StockQuantity StockQuantity { get; set; }
    }
}