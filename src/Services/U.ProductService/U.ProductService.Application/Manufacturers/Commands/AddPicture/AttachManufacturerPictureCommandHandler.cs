using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Manufacturers.Commands.AddPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AttachManufacturerPictureCommandHandler : IRequestHandler<AttachManufacturerPictureCommand>
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

        public async Task<Unit> Handle(AttachManufacturerPictureCommand command, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetAsync(command.ManufacturerId);

            if (manufacturer is null)
            {
                _logger.LogInformation($"Manufacturer with id: '{command.ManufacturerId}' has been not found");
                throw new ProductNotFoundException($"Manufacturer with id: '{command.ManufacturerId}' has not been found");
            }

            var picture = await _pictureRepository.GetAsync(command.PictureId);

            manufacturer.AttachPicture(picture.Id);

            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}