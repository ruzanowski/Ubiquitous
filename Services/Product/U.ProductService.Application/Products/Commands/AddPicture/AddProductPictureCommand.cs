using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace U.ProductService.Application.Products.Commands.AddPicture
{
    public class AddProductPictureCommand : IRequest<Guid>
    {
        [FromRoute] public Guid ProductId { get; set; }

        public string SeoFilename { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
        public string MimeType { get; set; }
    }
}