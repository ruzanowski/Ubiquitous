using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Persistance;

namespace U.ProductService.Application.Products.Queries.QueryProductByAlternativeKey
{
    public class ProductByAlternativeKeyQueryHandler : IRequestHandler<QueryProductByAlternativeKey, ProductViewModel>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public ProductByAlternativeKeyQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ProductViewModel> Handle(QueryProductByAlternativeKey request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Include(x => x.Pictures)
                .FirstOrDefaultAsync(x => x.CompareAlternateId(request.AlternativeKey),
                    cancellationToken);

            if (products is null)
                throw new ProductNotFoundException($"Product with alternative key: '{request.AlternativeKey}' has not been found.");
            
            var productsMapped = _mapper.Map<ProductViewModel>(products);

            return productsMapped;
        }
    }
}