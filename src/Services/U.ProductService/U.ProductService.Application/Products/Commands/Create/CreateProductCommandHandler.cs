﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Domain.Aggregates.Product;

namespace U.ProductService.Application.Products.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository,
            ILogger<CreateProductCommandHandler> logger, IManufacturerRepository manufacturerRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository;
            _logger = logger;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var duplicate = await _productRepository.GetByBarcodeAsync(command.BarCode);

            if (duplicate != null)
            {
                throw new ProductDuplicatedException($"Duplicated product with alternative key: '{command.BarCode}'");
            }

            if (command.CategoryId != null)
                await ValidateCategoryOrThrowAsync(command.CategoryId.Value);

            await ValidateManufacturerOrThrowAsync(command.ManufacturerId);

            var product = GetProduct(command);

            await _productRepository.AddAsync(product);

            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            _logger.LogInformation($"Product with id: '{product.Id}' has been created");

            return product.Id;
        }

        private Product GetProduct(CreateProductCommand command)
        {
            var dimensions = new Dimensions(command.Dimensions.Length,
                command.Dimensions.Width,
                command.Dimensions.Height,
                command.Dimensions.Weight);

            return new Product(command.Name,
                command.Price,
                command.BarCode,
                command.Description,
                dimensions,
                command.ManufacturerId,
                command.CategoryId ?? Category.GetDraftCategory().Id,
                ProductType.SimpleProduct.Id);
        }

        private async Task ValidateCategoryOrThrowAsync(Guid categoryId)
        {
            if (!await _categoryRepository.AnyAsync(categoryId))
            {
                throw new CategoryNotFoundException(
                    $"Category with given primary key: '{categoryId}' has not been found.");
            }
        }

        private async Task ValidateManufacturerOrThrowAsync(Guid id)
        {
            if (!await _manufacturerRepository.AnyAsync(id))
            {
                throw new ManufacturerNotFoundException(
                    $"Manufacturer with given primary key: '{id}' has not been found.");
            }
        }
    }
}