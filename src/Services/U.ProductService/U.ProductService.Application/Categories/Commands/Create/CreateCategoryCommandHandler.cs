using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.Categories.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository,
            IMediator mediator,
            IDomainEventsService domainEventsService,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
            _mapper = mapper;
        }

        public async Task<CategoryViewModel> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = GetCategory(command);

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

            var categoryViewModel = _mapper.Map<CategoryViewModel>(category);

            return categoryViewModel;
        }

        private Category GetCategory(CreateCategoryCommand command)
        {
            return new Category(Guid.NewGuid(),
                command.Name,
                command.Description,
                command.ParentId);
        }
    }
}