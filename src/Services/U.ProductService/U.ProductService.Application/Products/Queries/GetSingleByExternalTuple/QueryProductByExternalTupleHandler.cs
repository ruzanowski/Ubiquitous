using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Queries.GetSingleByExternalTuple
{
    public class ProductByExternalTupleQueryHandler : IRequestHandler<QueryProductByExternalTuple, ProductViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductByExternalTupleQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductViewModel> Handle(QueryProductByExternalTuple request, CancellationToken cancellationToken)
        {
            var productId = await _productRepository.GetIdByExternalTupleAsync(request.ExternalSourceName, request.ExternalSourceId);

            if (productId is null)
                throw new ProductNotFoundException(
                    $"Product with External Source Name: '{request.ExternalSourceName}' &&" +
                    $" External Source Id: '{request.ExternalSourceId}' has not been found.");

            var product = await _productRepository.GetAsync(productId.Value, false, cancellationToken);

            var productsMapped = _mapper.Map<ProductViewModel>(product);

            return productsMapped;
        }
    }
}