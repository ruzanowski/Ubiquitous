using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Exceptions;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Persistance;

namespace U.ProductService.Application.Pictures.Queries.QueryPicture
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
            
            var productsMapped = _mapper.Map<CategoryViewModel>(products);

            return productsMapped;
        }
    }
}