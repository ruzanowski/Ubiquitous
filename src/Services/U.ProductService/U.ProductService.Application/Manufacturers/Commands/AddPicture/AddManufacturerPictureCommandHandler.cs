using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Manufacturers.Commands.AddPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AddManufacturerPictureCommandHandler : IRequestHandler<AddManufacturerPictureCommand, Guid>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<AddManufacturerPictureCommandHandler> _logger;

        public AddManufacturerPictureCommandHandler(ILogger<AddManufacturerPictureCommandHandler> logger,
            IManufacturerRepository manufacturerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        public async Task<Guid> Handle(AddManufacturerPictureCommand command, CancellationToken cancellationToken)
        {
            var product = await _manufacturerRepository.GetAsync(command.ManufacturerId);

            if (product is null)
            {
                _logger.LogInformation($"Manufacturer with id: '{command.ManufacturerId}' has been not found");
                throw new ProductNotFoundException($"Manufacturer with id: '{command.ManufacturerId}' has not been found");
            }

            //todo: VALIDATION OF URL

            //todo: FILE STORAGE
            var fileStorageUploadId = Guid.NewGuid();
            var id = Guid.NewGuid();
            
            product.AddPicture(id, fileStorageUploadId, command.SeoFilename, command.Description, command.Url, command.MimeType);

            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return id;
        }
    }
}