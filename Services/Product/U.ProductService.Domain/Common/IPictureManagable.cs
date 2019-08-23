using System;
using System.Collections.Generic;

namespace U.ProductService.Domain
{
    public interface IPictureManagable
    {
        void AddPicture(Guid id, Guid fileStorageUploadId, string seoFilename, string description, string url, MimeType mimeType);
        void DeletePicture(Guid pictureId); 
        ICollection<Picture> Pictures { get; }
    }
}