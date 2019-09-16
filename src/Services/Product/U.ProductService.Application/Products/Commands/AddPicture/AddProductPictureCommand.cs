using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Picture;

namespace U.ProductService.Application.Products.Commands.AddPicture
{
    public class AddProductPictureCommand : IRequest<Guid>
    {
        [FromRoute] public Guid ProductId { get; set; }

        public string SeoFilename { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
        public MimeType MimeType { get; set; }
    }
}