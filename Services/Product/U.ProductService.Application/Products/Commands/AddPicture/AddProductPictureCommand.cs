using System;
using MediatR;
using Newtonsoft.Json;

namespace U.ProductService.Application.Products.Commands.AddPicture
{
    public class AddProductPictureCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid ProductId { get; private set; }

        public string SeoFilename { get; private set; }

        public string Description { get; private set; }

        public string Url { get; private set; }
        public string MimeType { get; private set; }

        public AddProductPictureCommand(Guid productId, string seoFilename, string description, string url, string mimeType)
        {
            ProductId = productId;
            SeoFilename = seoFilename;
            Description = description;
            Url = url;
            MimeType = mimeType;
        }
        public AddProductPictureCommand BindProductId(Guid productId)
        {
            ProductId = productId;
            return this;
        }
    }
}