using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.AddPicture
{
    public class AddManufacturerPictureCommand : IRequest<Guid>
    {
        [FromRoute] public Guid ManufacturerId { get; set; }

        public string SeoFilename { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
        public MimeType MimeType { get; set; }
    }
}