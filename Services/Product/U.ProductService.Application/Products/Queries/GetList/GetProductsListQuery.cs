using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetList
{
    public class GetProductsListQuery :  IPagination, IRequest<PaginatedItems<ProductViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}