using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.ProductCategories.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository,
            IMediator mediator,
             IDomainEventsService domainEventsService)
        {
            _categoryRepository = categoryRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
        }

        public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = GetCategory(command);

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

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