using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Guid> Handle(CreateProductCommand message, CancellationToken cancellationToken)
        {
            var product = new Product(message.Name, message.Price, message.BarCode, message.Description,
                message.Dimensions, Guid.NewGuid());

            await _productRepository.AddAsync(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            
            _logger.LogInformation("--- Creating Product: {@Product} ---", product.Id);

            return product.Id;
        }
    }
}