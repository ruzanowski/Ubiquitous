using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Products.Commands.AttachPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AttachPictureToProductCommandHandler : IRequestHandler<AttachPictureToProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public AttachPictureToProductCommandHandler(IProductRepository productRepository,
            IPictureRepository pictureRepository,
            IMediator mediator,
            IDomainEventsService domainEventsService)
        {
            _productRepository = productRepository;
            _pictureRepository = pictureRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
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
            await _productRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);
            await _productRepository.InvalidateCacheAsync(command.Id);
            return Unit.Value;
        }
    }
}