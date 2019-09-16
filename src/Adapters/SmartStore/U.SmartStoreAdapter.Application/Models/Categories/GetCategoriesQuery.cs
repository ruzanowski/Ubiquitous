using MediatR;
using U.Common.Pagination;

namespace U.SmartStoreAdapter.Application.Models.Categories
{
    public class GetCategoriesQuery : IRequest<PaginatedItems<CategoryViewModel>>, IPagination
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}