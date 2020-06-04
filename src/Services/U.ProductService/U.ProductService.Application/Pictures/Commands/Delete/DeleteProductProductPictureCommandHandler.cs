using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Pictures.Commands.Delete
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DeleteProductPictureCommandHandler : IRequestHandler<DeletePictureCommand>
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public DeleteProductPictureCommandHandler(IPictureRepository pictureRepository,
            IMediator mediator,
            IDomainEventsService domainEventsService)
        {
            _pictureRepository = pictureRepository;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
        }

        public async Task<Unit> Handle(DeletePictureCommand command, CancellationToken cancellationToken)
        {
            var picture = await _pictureRepository.GetAsync(command.PictureId);

            if (picture is null)
                throw new PictureNotFoundException($"Picture with id: '{command.PictureId}' has not been found");

            await _pictureRepository.Delete(command.PictureId);

            await _pictureRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);
            return Unit.Value;
        }
    }
}