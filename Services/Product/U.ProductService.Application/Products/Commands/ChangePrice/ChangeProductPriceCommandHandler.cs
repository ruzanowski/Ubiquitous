using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.ChangePrice
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ChangeProductPriceCommandHandler> _logger;

        public ChangeProductPriceCommandHandler(ILogger<ChangeProductPriceCommandHandler> logger,
            IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(ChangeProductPriceCommand message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(message.ProductId);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{message.ProductId}' has not been found");
            }

            product.ChangePrice(message.Price);

            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}