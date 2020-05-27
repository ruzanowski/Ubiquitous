using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Update
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            ILogger<UpdateProductCommandHandler> logger,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.ProductId);

            if (product is null)
            {
                _logger.LogInformation($"Product with id: '{command.ProductId}' has been not found");
                throw new ProductNotFoundException($"Product with id: '{command.ProductId}' has not been found");
            }

            var isUpdated = product.UpdateProperties(
                command.Name,
                command.Description,
                command.Price,
                new Dimensions(command.Dimensions.Length,
                    command.Dimensions.Width,
                    command.Dimensions.Height,
                    command.Dimensions.Weight),
                DateTime.UtcNow);

            if (!isUpdated)
            {
                return Unit.Value;
            }

            if (!command.QueuedJob?.AutoSave ?? false)
            {
                _productRepository.Update(product);
                await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }


            return Unit.Value;
        }
    }
}