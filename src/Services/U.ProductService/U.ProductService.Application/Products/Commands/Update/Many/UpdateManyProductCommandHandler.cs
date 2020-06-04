using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.Update.Many
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateManyProductCommandHandler : UpdateProductCommandHandlerBase, IRequestHandler<UpdateManyProductsCommand>
    {
        public async Task<Unit> Handle(UpdateManyProductsCommand command, CancellationToken cancellationToken)
        {
            foreach (var updateProductCommand in command.UpdateProductCommands)
            {
                await base.Handle(updateProductCommand, cancellationToken);
            }

            await ProductRepository.UnitOfWork.SaveEntitiesAsync(DomainEventsService, Mediator, cancellationToken);

            return Unit.Value;
        }

        public UpdateManyProductCommandHandler(IProductRepository productRepository, IMediator mediator, IDomainEventsService domainEventsService) : base(productRepository, mediator, domainEventsService)
        {
        }
    }
}