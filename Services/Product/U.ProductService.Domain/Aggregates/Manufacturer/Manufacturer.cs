using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain
{
    /// <summary>
    /// Represents a manufacturer
    /// </summary>
    [DataContract]
    public class Manufacturer : Entity, IAggregateRoot, ITrackable, IPictureManagable
    {
        public Guid AggregateId => Id;
        public string AggregateTypeName => nameof(Manufacturer);
        public string Name { get; private set; }
        public string Description { get; private set; }

        private DateTime _createdAt;
        private string _createdBy;
        private DateTime? _lastUpdatedAt;
        private string _lastUpdatedBy;
        public DateTime CreatedAt => _createdAt;
        public string CreatedBy => _createdBy;
        public DateTime? LastUpdatedAt => _lastUpdatedAt;
        public string LastUpdatedBy => _lastUpdatedBy;
        public ICollection<Picture> Pictures { get; private set; }
        
        private Manufacturer()
        {
            Name = string.Empty;
            Description = string.Empty;
            _createdAt = DateTime.UtcNow;
            _createdBy = string.Empty;
            _lastUpdatedAt = default;
            _lastUpdatedBy = string.Empty;
        }
        
        public Manufacturer(Guid id, string name, string description) : this()
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

            var picture = new Picture(id, AggregateId, AggregateTypeName, fileStorageUploadId, seoFilename, description, url, mimeType);

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