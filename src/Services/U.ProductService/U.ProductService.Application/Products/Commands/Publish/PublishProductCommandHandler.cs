using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Publish
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PublishProductCommandHandler : IRequestHandler<PublishProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public PublishProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}