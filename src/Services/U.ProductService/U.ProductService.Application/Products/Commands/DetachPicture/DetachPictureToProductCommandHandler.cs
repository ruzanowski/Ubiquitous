using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Commands.DetachPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DetachPictureToProductCommandHandler : IRequestHandler<DetachPictureToProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductContext _context;

        public DetachPictureToProductCommandHandler(IProductRepository productRepository, ProductContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<Unit> Handle(DetachPictureToProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.Id, false, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found.");
            }

            product.DetachPicture(command.PictureId);

            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            await _productRepository.InvalidateCache(command.Id);
            return Unit.Value;
        }
    }
}