using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoriesListQuery, PaginatedItems<CategoryViewModel>>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetCategoriesListQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<CategoryViewModel>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var categories = _context.Categories.AsQueryable();

            var categoriesMapped = _mapper.ProjectTo<CategoryViewModel>(categories);

            var paginatedCategories =
                PaginatedItems<CategoryViewModel>.Create(request.PageIndex, request.PageSize, categoriesMapped);

            await Task.CompletedTask;

            return paginatedCategories;
        }
    }
}