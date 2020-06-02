using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.ProductCategories.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.ProductCategories.Queries.GetProductCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetProductCategoryQuery, ProductCategoryViewModel>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductCategoryViewModel> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.ProductCategories.FindAsync(request.Id);

            if (products is null)
                throw new ProductCategoryNotFoundException($"ProductCategory with primary key: '{request.Id}' has not been found.");

            var mapped = _mapper.Map<ProductCategoryViewModel>(products);

            return mapped;
        }
    }
}