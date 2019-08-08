using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.Update
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(UpdateProductCommand message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(message.ProductId);
            
            if (product is null)
            {
                _logger.LogInformation($"Product with id: '{message.ProductId}' has been not found");
                throw new ProductNotFoundException($"Product with id: '{message.ProductId}' has not been found");
            }

            _logger.LogInformation("--- Updating Product: {@Product} ---", product.Id);

            var dimensions = new Dimensions(message.Dimensions.Length,
                message.Dimensions.Width,
                message.Dimensions.Height,
                message.Dimensions.Weight);

            product.UpdateAllProperties(message.Name, message.Price, dimensions, DateTime.UtcNow);
            
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}