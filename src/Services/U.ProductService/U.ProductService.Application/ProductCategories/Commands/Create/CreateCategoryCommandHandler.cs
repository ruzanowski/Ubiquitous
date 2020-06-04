using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.ProductCategories.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = GetCategory(command);

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return category.Id;
        }

        private ProductCategory GetCategory(CreateCategoryCommand command)
        {
            return new ProductCategory(Guid.NewGuid(),
                command.Name,
                command.Description,
                command.ParentId);
        }
    }
}