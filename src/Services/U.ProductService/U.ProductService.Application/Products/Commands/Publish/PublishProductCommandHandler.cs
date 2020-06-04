using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.Publish
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PublishProductCommandHandler : IRequestHandler<PublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public PublishProductCommandHandler(IProductRepository productRepository,
            IMediator mediator,
            IDomainEventsService domainEventsService)
        {
            _productRepository = productRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
        }

        public async Task<Unit> Handle(PublishProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.Id, false, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found.");
            }

            product.Publish();
            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

            return Unit.Value;
        }
    }
}