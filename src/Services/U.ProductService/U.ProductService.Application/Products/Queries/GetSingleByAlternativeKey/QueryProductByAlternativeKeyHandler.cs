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
            var productId = await _productRepository.GetAggregateIdByAbsoluteComparerAsync(request.ExternalSourceName, request.ExternalSourceId);

            if (productId is null)
                throw new ProductNotFoundException(
                    $"Product with externalSourceName: '{request.ExternalSourceName}' &" +
                    $" externalSourceId: '{request.ExternalSourceId}' has not been found.");

            var product = await _productRepository.GetAsync(productId.Value);

            var productsMapped = _mapper.Map<ProductViewModel>(product);

            return productsMapped;
        }
    }
}