using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<CreateProductCommandHandler> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var duplicate = await _productRepository.AnyAlternateIdAsync(command.BarCode);

            if (duplicate)
            {
                throw new ProductDuplicatedException($"Duplicated product with alternative key: '{command.BarCode}'");
            }
            
            var categoryId = await GetCategoryOrThrowAsync(command);

            var product = GetProduct(command, categoryId);
            
            await _productRepository.AddAsync(product); 
            
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            _logger.LogInformation($"Product with id: '{product.Id}' has been created");

            return product.Id;
        }

        private Product GetProduct(CreateProductCommand command, Guid categoryId)
        {
            return new Product(command.Name,
                command.Price,
                command.BarCode,
                command.Description,
                new Dimensions(command.Dimensions.Length,
                    command.Dimensions.Width,
                    command.Dimensions.Height,
                    command.Dimensions.Weight),
                Guid.NewGuid(),
                categoryId,
                ProductType.SimpleProduct.Id);
        }

        private async Task<Guid> GetCategoryOrThrowAsync(CreateProductCommand command)
        {
            Guid categoryId;
            
            if (command.CategoryId.HasValue)
            {
                if (!await _categoryRepository.AnyAsync(command.CategoryId.Value))
                {
                    throw new CategoryNotFoundException(
                        $"Category with given primary key: '{command.CategoryId}' has not been found.");
                }
                categoryId = command.CategoryId.Value;
            }
            else
            {
                var category = await _categoryRepository.GetOrCreateDraftCategoryAsync();
                categoryId = category.Id;
            }

            return categoryId;
        }
        
    }
}