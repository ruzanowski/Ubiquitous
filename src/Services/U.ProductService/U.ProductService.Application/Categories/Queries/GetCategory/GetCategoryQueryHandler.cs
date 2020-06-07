using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Categories.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryViewModel>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryViewModel> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Categories.FindAsync(request.Id);

            if (products is null)
                throw new CategoryNotFoundException($"Category with primary key: '{request.Id}' has not been found.");

            var mapped = _mapper.Map<CategoryViewModel>(products);

            return mapped;
        }
    }
}