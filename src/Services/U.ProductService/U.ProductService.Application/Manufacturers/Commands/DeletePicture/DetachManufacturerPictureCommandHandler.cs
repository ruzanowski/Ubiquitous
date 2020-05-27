using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Manufacturers.Commands.DeletePicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DetachManufacturerPictureCommandHandler : IRequestHandler<DetachManufacturerPictureCommand>
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public DetachManufacturerPictureCommandHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Unit> Handle(DetachManufacturerPictureCommand command, CancellationToken cancellationToken)
        {
            var product = await _manufacturerRepository.GetAsync(command.ManufacturerId);

            if (product is null)
                throw new ProductNotFoundException($"Product with id: '{command.ManufacturerId}' has not been found");

            //todo: VALIDATION OF URL

            //todo: FILE STORAGE

            product.DetachPicture(command.PictureId);
            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}