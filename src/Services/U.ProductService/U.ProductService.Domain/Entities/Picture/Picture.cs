using System;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Entities.Picture
{
    public class Picture : Entity
    {
        public Guid FileStorageUploadId { get; private set; }
        public string FileName { get; private set; }
        public string Description { get; private set; }
        public string Url { get; private set; }
        public MimeType MimeType { get; private set; }
        public int MimeTypeId { get; private set; }
        public DateTime PictureAddedAt { get; private set; }

        private Picture()
        {
            PictureAddedAt = DateTime.UtcNow;
        }

        public Picture(Guid fileStorageUploadId,
            string fileName,
            string description,
            string url,
            int mimeTypeId) : this()
        {
            FileStorageUploadId = fileStorageUploadId;
            FileName = fileName;
            Description = description;
            Url = url;
            MimeTypeId = mimeTypeId;
        }

        public void Update(Guid fileStorageUploadId,
            string fileName,
            string description,
            string url,
            int mimeTypeId)
        {
            FileStorageUploadId = fileStorageUploadId;
            FileName = fileName;
            Description = description;
            Url = url;
            MimeTypeId = mimeTypeId;
        }
    }
}