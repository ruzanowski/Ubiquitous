using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Manufacturers.Commands.DetachPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DetachManufacturerPictureCommandHandler : IRequestHandler<DetachPictureFromManufacturerCommand>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;
        private readonly IPictureRepository _pictureRepository;

        public DetachManufacturerPictureCommandHandler(IManufacturerRepository manufacturerRepository,
            IMediator mediator,
            IDomainEventsService domainEventsService,
            IPictureRepository pictureRepository)
        {
            _manufacturerRepository = manufacturerRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
            _pictureRepository = pictureRepository;
        }

        public async Task<Unit> Handle(DetachPictureFromManufacturerCommand command, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetAsync(command.ManufacturerId, false);

            if (manufacturer is null)
                throw new ProductNotFoundException($"Product with id: '{command.ManufacturerId}' has not been found");

            var picture = await _pictureRepository.GetAsync(command.PictureId);


            if (picture is null)
            {
                throw new PictureNotFoundException($"Picture with id: '{command.PictureId}' has not been found.");
            }

            //todo: VALIDATION OF URL

            //todo: FILE STORAGE

            manufacturer.DetachPicture(command.PictureId);
            _manufacturerRepository.Update(manufacturer);
            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);
            await _manufacturerRepository.InvalidateCacheAsync(manufacturer.Id);
            return Unit.Value;
        }
    }
}