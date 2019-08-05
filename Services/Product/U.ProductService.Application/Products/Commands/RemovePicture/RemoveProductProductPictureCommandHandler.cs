using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Application.Products.Commands.AddPicture;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.RemovePicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class RemoveProductPictureCommandHandler : IRequestHandler<RemoveProductPictureCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<RemoveProductPictureCommandHandler> _logger;

        public RemoveProductPictureCommandHandler(ILogger<RemoveProductPictureCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<bool> Handle(RemoveProductPictureCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.ProductId);
            
            if (product is null)
            {
                _logger.LogInformation($"Product with id: '{command.ProductId}' has been not found");
                throw new ProductNotFoundException($"Product with id: '{command.ProductId}' has not been found");
            }

            _logger.LogInformation("--- Adding product picture: {@Product} ---", product.Id);
            
            //todo: VALIDATION OF URL
            
            //todo: FILE STORAGE

            product.RemovePicture(command.ProductId);
            
            return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}