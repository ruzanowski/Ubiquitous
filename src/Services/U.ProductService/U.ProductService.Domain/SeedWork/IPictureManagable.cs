using System;
using System.Collections.Generic;
using U.ProductService.Domain.Aggregates.Picture;

namespace U.ProductService.Domain.SeedWork
{
    public interface IPictureManagable
    {
        void AddPicture(Guid id, Guid fileStorageUploadId, string filename, string description, string url, MimeType mimeType);
        void DeletePicture(Guid pictureId);
        ICollection<Picture> Pictures { get; }
    }
}