using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.Create.Many
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateManyProductsCommandHandler : CreateProductCommandHandlerBase,
        IRequestHandler<CreateManyProductsCommand>
    {
        public CreateManyProductsCommandHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IManufacturerRepository manufacturerRepository,
            IMapper mapper,
            IMediator mediator,
            IDomainEventsService domainEventsService) : base(productRepository, categoryRepository,
            manufacturerRepository, mapper, mediator, domainEventsService)
        {
        }

        public async Task<Unit> Handle(CreateManyProductsCommand request, CancellationToken cancellationToken)
        {
            foreach (var createProductCommand in request.CreateProductCommands)
            {
                await base.Handle(createProductCommand, cancellationToken);
            }

            await ProductRepository.UnitOfWork.SaveEntitiesAsync(DomainEventsService, Mediator, cancellationToken);

            return Unit.Value;
        }
    }
}