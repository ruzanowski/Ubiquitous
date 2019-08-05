using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Application.Products.Commands.CreateProduct;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.PublishProduct
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PublishProductCommandHandler : IRequestHandler<PublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public PublishProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(PublishProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.Id);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found.");
            }
            
            product.Publish();
            
            _logger.LogInformation("--- Publishing Product: {@Product} ---", product.Id);

            await _productRepository.AddAsync(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}