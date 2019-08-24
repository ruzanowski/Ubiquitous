using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Queries.QueryProductByAlternativeKey
{
    public class ProductByAlternativeKeyQueryHandler : IRequestHandler<QueryProductByAlternativeKey, ProductViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductByAlternativeKeyQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        
        public async Task<ProductViewModel> Handle(QueryProductByAlternativeKey request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByAlternativeIdAsync(request.AlternativeKey);

            if (products is null)
                throw new ProductNotFoundException($"Product with alternative key: '{request.AlternativeKey}' has not been found.");
            
            var productsMapped = _mapper.Map<ProductViewModel>(products);

            return productsMapped;
        }
    }
}