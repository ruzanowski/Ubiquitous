using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.UnPublish
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UnPublishProductCommandHandler : IRequestHandler<UnPublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UnPublishProductCommandHandler> _logger;

        public UnPublishProductCommandHandler(ILogger<UnPublishProductCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(UnPublishProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.Id);

            if (product is null)
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found.");

            product.UnPublish();
            
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}