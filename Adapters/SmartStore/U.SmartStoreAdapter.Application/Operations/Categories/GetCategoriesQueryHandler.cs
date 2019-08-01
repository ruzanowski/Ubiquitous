using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SmartStore.Persistance.Context;
using U.Common.Pagination;
using U.SmartStoreAdapter.Api.Categories;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Operations.Categories
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
                await PaginatedItems<CategoryViewModel>.PaginatedItemsCreate.CreateAsync(request.PageIndex,
                    request.PageSize, productsMapped);

            return paginatedProducts;
        }
    }
    
}