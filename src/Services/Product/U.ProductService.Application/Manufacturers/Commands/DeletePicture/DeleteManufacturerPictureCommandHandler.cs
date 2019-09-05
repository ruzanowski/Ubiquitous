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
    public class DeleteManufacturerPictureCommandHandler : IRequestHandler<DeleteManufacturerPictureCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteManufacturerPictureCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Unit> Handle(DeleteManufacturerPictureCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(command.ManufacturerId);
            
            if (product is null)
                throw new ProductNotFoundException($"Product with id: '{command.ManufacturerId}' has not been found");

            //todo: VALIDATION OF URL
            
            //todo: FILE STORAGE

            product.DeletePicture(command.PictureId);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}