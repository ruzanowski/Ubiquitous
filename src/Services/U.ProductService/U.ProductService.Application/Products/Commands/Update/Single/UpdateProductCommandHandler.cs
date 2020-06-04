using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.Update.Single
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductCommandHandler : UpdateProductCommandHandlerBase, IRequestHandler<UpdateProductCommand>
    {
        public new async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            => await base.Handle(command, cancellationToken);

        public UpdateProductCommandHandler(IProductRepository productRepository, IMediator mediator, IDomainEventsService domainEventsService) : base(productRepository, mediator, domainEventsService)
        {
        }
    }
}