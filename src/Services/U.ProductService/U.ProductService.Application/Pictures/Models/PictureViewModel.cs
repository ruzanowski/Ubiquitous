using System;

namespace U.ProductService.Application.Pictures.Models
{
    public class PictureViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int MimeTypeId { get; set; }
        public DateTime PictureAddedAt { get; set; }
    }
}