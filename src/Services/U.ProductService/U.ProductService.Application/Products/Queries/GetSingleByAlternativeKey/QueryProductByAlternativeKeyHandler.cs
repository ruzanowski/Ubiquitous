using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Queries.GetSingleByAlternativeKey
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
            var products = await _productRepository.GetByAbsoluteComparerAsync(request.ExternalSourceName, request.ExternalSourceId);

            if (products is null)
                throw new ProductNotFoundException(
                    $"Product with externalSourceName: '{request.ExternalSourceName}' &" +
                    $" externalSourceId: '{request.ExternalSourceId}' has not been found.");

            var productsMapped = _mapper.Map<ProductViewModel>(products);

            return productsMapped;
        }
    }
}