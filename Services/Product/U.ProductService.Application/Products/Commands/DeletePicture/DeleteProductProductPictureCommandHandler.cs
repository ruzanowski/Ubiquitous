using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.DeletePicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DeleteProductPictureCommandHandler : IRequestHandler<DeleteProductPictureCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteProductPictureCommandHandler> _logger;

        public DeleteProductPictureCommandHandler(ILogger<DeleteProductPictureCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(DeleteProductPictureCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.ProductId);
            
            if (product is null)
                throw new ProductNotFoundException($"Product with id: '{command.ProductId}' has not been found");

            //todo: VALIDATION OF URL
            
            //todo: FILE STORAGE

            product.DeletePicture(command.PictureId);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}