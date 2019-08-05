﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Products.Commands.AddPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AddProductPictureCommandHandler : IRequestHandler<AddProductPictureCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<AddProductPictureCommandHandler> _logger;

        public AddProductPictureCommandHandler(ILogger<AddProductPictureCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(AddProductPictureCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.ProductId);
            
            if (product is null)
            {
                _logger.LogInformation($"Product with id: '{command.ProductId}' has been not found");
                throw new ProductNotFoundException($"Product with id: '{command.ProductId}' has not been found");
            }

            _logger.LogInformation("--- Adding product picture: {@Product} ---", command.ProductId);
            
            //todo: VALIDATION OF URL
            
            //todo: FILE STORAGE

             product.AddPicture(command.SeoFilename, command.Description, command.Url, command.MimeType);
             var pictureId = product.Pictures.Last().Id;
             
             await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
             return Unit.Value;
        }
    }
}