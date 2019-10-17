using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.GetSingle
{
    public class GetProductQueryHandler : IRequestHandler<QueryProduct, ProductViewModel>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ProductViewModel> Handle(QueryProduct request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Include(x => x.Pictures)
                .Include(x=>x.Category)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id),
                    cancellationToken);

            if (products is null)
                throw new ProductNotFoundException($"Product with primary key: '{request.Id}' has not been found.");
            
            var productsMapped = _mapper.Map<ProductViewModel>(products);

            return productsMapped;
        }
    }
}