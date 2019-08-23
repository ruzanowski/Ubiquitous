using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.ChangeCategory
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ChangeProductCategoryCommandHandler : IRequestHandler<ChangeProductCategoryCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ChangeProductCategoryCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(ChangeProductCategoryCommand message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(message.ProductId);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{message.ProductId}' has not been found");
            }

            var categoryExists = await _categoryRepository.AnyAsync(message.CategoryId);

            if (!categoryExists)
            {
                throw new CategoryNotFoundException($"Category with id: '{message.CategoryId}' has not been found");
            }
            
            product.ChangeCategory(message.CategoryId);

            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}