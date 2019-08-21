using System;
using System.Collections.Generic;
using System.Linq;
using U.ProductService.Domain.Events;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain
{

	public class Category : Entity, IPictureManagable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<Picture> Pictures { get; private set; }
        
        private Category()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
        }
        
        public Category(Guid id, string name, string description) : this()
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public void AddPicture(Guid id, Guid fileStorageUploadId, string seoFilename, string description, string url, MimeType mimeType)
        {
            if (string.IsNullOrEmpty(seoFilename))
                throw new ProductDomainException($"{nameof(seoFilename)} cannot be null or empty!");

            if (string.IsNullOrEmpty(description))
                throw new ProductDomainException($"{nameof(description)} cannot be null or empty!");

            if (string.IsNullOrEmpty(url))
                throw new ProductDomainException($"{nameof(url)} cannot be null or empty!");

            var picture = new Picture(id, fileStorageUploadId, seoFilename, description, url,  mimeType);

            Pictures.Add(picture);
        }

        public void DeletePicture(Guid pictureId)
        {
            var picture = Pictures.FirstOrDefault(x => x.Id.Equals(pictureId));

            if (picture is null)
                throw new ProductDomainException("Picture does not exist!");

            Pictures.Remove(picture);
        }
    }
}
