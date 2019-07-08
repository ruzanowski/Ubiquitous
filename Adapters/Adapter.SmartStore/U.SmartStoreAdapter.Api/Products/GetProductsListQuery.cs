using MediatR;
using U.Common;
using U.SmartStoreAdapter.Api.QueryModels;

namespace U.SmartStoreAdapter.Api.Products
{
    public class GetProductsListQuery :  IRequest<PaginatedItems<SmartProductViewModel>>, IPagination
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public ProductOrderBy OrderBy { get; set; }
        public PriceWindow Price { get; set; }
        public StockQuantity StockQuantity { get; set; }
        public TimeWindow Time { get; set; } //time of what?
        public string Category { get; set; }
    }
}