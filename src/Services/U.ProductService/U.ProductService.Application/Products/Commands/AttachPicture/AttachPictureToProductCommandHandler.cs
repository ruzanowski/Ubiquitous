using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Products.Commands.AttachPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AttachPictureToProductCommandHandler : IRequestHandler<AttachPictureToProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPictureRepository _pictureRepository;

        public AttachPictureToProductCommandHandler(IProductRepository productRepository, IPictureRepository pictureRepository)
        {
            _productRepository = productRepository;
            _pictureRepository = pictureRepository;
        }

        public async Task<Unit> Handle(AttachPictureToProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.Id, false, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found.");
            }

            var picture = await _pictureRepository.GetAsync(command.PictureId);

            if (picture is null)
            {
                throw new PictureNotFoundException($"Picture with id: '{command.PictureId}' has not been found.");
            }

            product.AttachPicture(command.PictureId);
            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            await _productRepository.InvalidateCache(command.Id);
            return Unit.Value;
        }
    }
}