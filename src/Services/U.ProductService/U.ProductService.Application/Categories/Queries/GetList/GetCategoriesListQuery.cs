using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Categories.Models;

namespace U.ProductService.Application.Categories.Queries.GetList
{
    public class GetCategoriesListQuery :  IPagination, IRequest<PaginatedItems<CategoryViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}