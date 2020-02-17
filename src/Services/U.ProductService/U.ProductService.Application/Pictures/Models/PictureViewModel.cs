using System;

namespace U.ProductService.Application.Pictures.Models
{
    public class PictureViewModel
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string Description { get; private set; }
        public string Url { get; private set; }
        public string MimeType { get; private set; }
    }
}