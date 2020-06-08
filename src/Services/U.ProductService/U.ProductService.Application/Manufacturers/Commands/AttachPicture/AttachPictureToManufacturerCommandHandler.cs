using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Manufacturers.Commands.AttachPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AttachManufacturerPictureCommandHandler : IRequestHandler<AttachPictureToManufacturerCommand>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly ILogger<AttachManufacturerPictureCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public AttachManufacturerPictureCommandHandler(ILogger<AttachManufacturerPictureCommandHandler> logger,
            IManufacturerRepository manufacturerRepository,
            IPictureRepository pictureRepository,
            IMediator mediator, IDomainEventsService domainEventsService)
        {
            _logger = logger;
            _manufacturerRepository = manufacturerRepository;
            _pictureRepository = pictureRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
        }

        public async Task<Unit> Handle(AttachPictureToManufacturerCommand command, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetAsync(command.ManufacturerId, false);

            if (manufacturer is null)
            {
                _logger.LogInformation($"Manufacturer with id: '{command.ManufacturerId}' has been not found");
                throw new ProductNotFoundException($"Manufacturer with id: '{command.ManufacturerId}' has not been found");
            }

            var picture = await _pictureRepository.GetAsync(command.PictureId);


            if (picture is null)
            {
                throw new PictureNotFoundException($"Picture with id: '{command.PictureId}' has not been found.");
            }

            manufacturer.AttachPicture(picture.Id);
            _manufacturerRepository.Update(manufacturer);

            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken: cancellationToken);
            await _manufacturerRepository.InvalidateCacheAsync(manufacturer.Id);

            return Unit.Value;
        }
    }
}