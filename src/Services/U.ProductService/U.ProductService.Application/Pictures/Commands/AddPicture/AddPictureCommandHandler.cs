using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Domain.Entities.Picture;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Pictures.Commands.AddPicture
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AddPictureCommandHandler : IRequestHandler<AddPictureCommand, PictureViewModel>
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public AddPictureCommandHandler(IPictureRepository productRepository,
            IMapper mapper)
        {
            _pictureRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PictureViewModel> Handle(AddPictureCommand command, CancellationToken cancellationToken)
        {
            var validator = new AddPictureCommand.Validator();
            await validator.ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

            var picture = new Picture(Guid.NewGuid(),
                command.Filename,
                command.Description,
                command.Url,
                command.MimeTypeId);

            await _pictureRepository.AddAsync(picture);

            await _pictureRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return _mapper.Map<PictureViewModel>(picture);
        }
    }
}