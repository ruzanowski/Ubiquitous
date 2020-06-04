using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.UnPublish
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UnPublishProductCommandHandler : IRequestHandler<UnPublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public UnPublishProductCommandHandler(
            IProductRepository productRepository,
            IMediator mediator,
            IDomainEventsService domainEventsService)
        {
            _productRepository = productRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
        }

        public async Task<Unit> Handle(UnPublishProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.Id, false, cancellationToken);

            if (product is null)
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found.");

            product.Unpublish();
            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

            return Unit.Value;
        }
    }
}