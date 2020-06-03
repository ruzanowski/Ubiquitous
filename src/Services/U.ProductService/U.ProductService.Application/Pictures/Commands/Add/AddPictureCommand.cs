using FluentValidation;
using MediatR;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Pictures.Commands.AddPicture
{
    public class AddPictureCommand : IRequest<PictureViewModel>
    {
        public string Filename { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int MimeTypeId { get; set; }

        public class Validator : AbstractValidator<AddPictureCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Filename).NotEmpty();
                RuleFor(x => x.Url).NotEmpty();
                RuleFor(x => x.MimeTypeId).NotEmpty();
            }
        }
    }
}