using MediatR;
using U.Common.Pagination;
using U.SmartStoreAdapter.Application.Models.QueryModels;

namespace U.SmartStoreAdapter.Application.Models.Products
{
    public class GetProductsListQuery :  IRequest<PaginatedItems<SmartProductViewModel>>, IPagination
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
        public ProductOrderBy OrderBy { get; set; }
        public PriceWindow Price { get; set; }
        public StockQuantity StockQuantity { get; set; }
        public TimeWindow Time { get; set; } //time of what?
        public string Category { get; set; }
    }
}