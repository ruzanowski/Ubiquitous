using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.ChangePrice
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ChangeProductPriceCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public ChangeProductPriceCommandHandler(ILogger<ChangeProductPriceCommandHandler> logger,
            IProductRepository productRepository,
            IDomainEventsService domainEventsService,
            IMediator mediator)
        {
            _logger = logger;
            _productRepository = productRepository;
            _domainEventsService = domainEventsService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ChangeProductPriceCommand message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(message.Id, false, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{message.Id}' has not been found");
            }

            product.ChangePrice(message.Price);
            _productRepository.Update(product);

            await _productRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);
            return Unit.Value;
        }
    }
}