using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.ProductCategories.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.ProductCategories.Queries.GetProductCategories
{
    public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoriesListQuery, PaginatedItems<ProductCategoryViewModel>>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetCategoriesListQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<ProductCategoryViewModel>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var categories = _context.ProductCategories.AsQueryable();

            var categoriesMapped = _mapper.ProjectTo<ProductCategoryViewModel>(categories);

            var paginatedCategories =
                PaginatedItems<ProductCategoryViewModel>.Create(request.PageIndex, request.PageSize, categoriesMapped);

            await Task.CompletedTask;

            return paginatedCategories;
        }
    }
}