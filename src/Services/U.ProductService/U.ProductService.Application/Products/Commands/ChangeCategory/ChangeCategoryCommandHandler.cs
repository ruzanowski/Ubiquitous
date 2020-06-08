using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.ChangeCategory
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ChangeCategoryCommandHandler : IRequestHandler<ChangeCategoryCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public ChangeCategoryCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(ChangeCategoryCommand message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(message.ProductId, false, cancellationToken);

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
            _productRepository.Update(product);

            await _productRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);
            return Unit.Value;
        }
    }
}