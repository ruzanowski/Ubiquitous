using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Queries.GetSingle
{
    public class GetProductQueryHandler : IRequestHandler<QueryProduct, ProductViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductViewModel> Handle(QueryProduct request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAsync(request.Id, true, cancellationToken);

            if (products is null)
                throw new ProductNotFoundException($"Product with primary key: '{request.Id}' has not been found.");

            var productsMapped = _mapper.Map<ProductViewModel>(products);

            return productsMapped;
        }
    }
}