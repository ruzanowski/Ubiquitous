using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.UpdateProduct
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateProductCommand> _logger;

        public UpdateProductCommandHandler(ILogger<UpdateProductCommand> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<bool> Handle(UpdateProductCommand message, CancellationToken cancellationToken)
        {
            Product product;
            
            if (message.ProductId.HasValue)
            {
                product = await _productRepository.GetAsync(message.ProductId.Value);
            }            
            else if (message.AlternateId != null)
            {
                product = await _productRepository.GetAlternateIdAsync(message.AlternateId);
            }
            else
            {
                _logger.LogInformation($"Product with id: '{message.ProductId ?? Guid.Empty}' and alternateId '{message.AlternateId}'");
                throw new ProductNotFoundException(
                    $"Product with id: '{message.ProductId ?? Guid.Empty}' and alternateId '{message.AlternateId}'");
            }

            _logger.LogInformation("--- Updating Product: {@Product} ---", product);

            var generatedTime = DateTime.UtcNow;

            product.UpdateAllProperties(message.Name, message.Price, message.Dimensions, generatedTime);
            
            return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}