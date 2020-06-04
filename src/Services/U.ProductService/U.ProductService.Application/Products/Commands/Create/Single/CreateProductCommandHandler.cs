using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.Create.Single
{
    public class CreateProductCommandHandler : CreateProductCommandHandlerBase,
        IRequestHandler<CreateProductCommand, ProductViewModel>
    {
        public new async Task<ProductViewModel> Handle(CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            return await base.Handle(command, cancellationToken);
        }

        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository,
            IManufacturerRepository manufacturerRepository, IMapper mapper, IMediator mediator,
            IDomainEventsService domainEventsService) : base(productRepository, categoryRepository,
            manufacturerRepository, mapper, mediator, domainEventsService)
        {
        }
    }
}