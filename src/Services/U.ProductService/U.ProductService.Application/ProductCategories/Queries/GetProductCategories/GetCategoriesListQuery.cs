using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.ProductCategories.Models;

namespace U.ProductService.Application.ProductCategories.Queries.GetProductCategories
{
    public class GetCategoriesListQuery :  IPagination, IRequest<PaginatedItems<ProductCategoryViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}