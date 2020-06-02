using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SmartStore.Persistance.Context;
using U.Common.Pagination;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Categories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedItems<CategoryViewModel>>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(SmartStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<CategoryViewModel>> Handle(GetCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var products = _context.Set<Category>().AsQueryable();

            var productsMapped = _mapper.ProjectTo<CategoryViewModel>(products);

            var paginatedProducts =
                PaginatedItems<CategoryViewModel>.Create(request.PageIndex,
                    request.PageSize, productsMapped);

            await Task.CompletedTask;

            return paginatedProducts;
        }
    }

}