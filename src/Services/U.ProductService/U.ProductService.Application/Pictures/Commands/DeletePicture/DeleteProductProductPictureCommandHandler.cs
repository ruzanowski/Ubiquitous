using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Pictures.Commands.DeletePicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DeleteProductPictureCommandHandler : IRequestHandler<DeletePictureCommand>
    {
        private readonly IPictureRepository _pictureRepository;

        public DeleteProductPictureCommandHandler(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<Unit> Handle(DeletePictureCommand command, CancellationToken cancellationToken)
        {
            var picture = await _pictureRepository.GetAsync(command.PictureId);

            if (picture is null)
                throw new PictureNotFoundException($"Picture with id: '{command.PictureId}' has not been found");

            await _pictureRepository.Delete(command.PictureId);

            await _pictureRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}