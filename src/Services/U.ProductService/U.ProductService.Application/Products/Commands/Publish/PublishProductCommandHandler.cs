using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Publish
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PublishProductCommandHandler : IRequestHandler<PublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<PublishProductCommandHandler> _logger;

        public PublishProductCommandHandler(ILogger<PublishProductCommandHandler> logger, IProductRepository productRepository)
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
            
            _logger.LogInformation("--- Publishing Product: {@Product} ---", command.Id);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}