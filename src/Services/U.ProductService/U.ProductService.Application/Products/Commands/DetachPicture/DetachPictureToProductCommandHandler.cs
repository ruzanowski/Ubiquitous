using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Products.Commands.DetachPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DetachPictureToProductCommandHandler : IRequestHandler<DetachPictureToProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;
        private readonly IPictureRepository _pictureRepository;

        public DetachPictureToProductCommandHandler(IProductRepository productRepository,
            IDomainEventsService domainEventsService,
            IMediator mediator,
            IPictureRepository pictureRepository)
        {
            _productRepository = productRepository;
            _domainEventsService = domainEventsService;
            _mediator = mediator;
            _pictureRepository = pictureRepository;
        }

        public async Task<Unit> Handle(DetachPictureToProductCommand command, CancellationToken cancellationToken)
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

            product.DetachPicture(command.PictureId);

            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

            await _productRepository.InvalidateCacheAsync(command.Id);
            return Unit.Value;
        }
    }
}