using MediatR;
using U.Common;

namespace U.SmartStoreAdapter.Api.Categories
{
    public class GetCategoriesQuery : IRequest<PaginatedItems<CategoryViewModel>>, IPagination
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}