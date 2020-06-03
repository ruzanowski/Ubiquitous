using System;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using U.Common.NetCore.Swagger;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Pictures.Commands.AddPicture
{
    public class UpdatePictureCommand : IRequest<PictureViewModel>
    {
        [JsonIgnore]
        [SwaggerExclude]
        public Guid PictureId { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public Guid FileStorageUploadId { get; set; }
        public string Url { get; set; }
        public int MimeTypeId { get; set; }

        public class Validator : AbstractValidator<UpdatePictureCommand>
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