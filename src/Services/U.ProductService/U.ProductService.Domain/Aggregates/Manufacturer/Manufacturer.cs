using System;
using System.Collections.Generic;
using System.Linq;
using U.ProductService.Domain.Aggregates.Picture;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Aggregates.Manufacturer
{
    /// <summary>
    /// Represents a manufacturer
    /// </summary>
    public class Manufacturer : Entity, IAggregateRoot, ITrackable, IPictureManagable
    {
        public Guid AggregateId => Id;
        public string AggregateTypeName => nameof(Manufacturer);
        public string Name { get; private set; }
        public string Description { get;  set; }
        public string UniqueClientId { get;  set; }

        private DateTime _createdAt;
        private string _createdBy;
        private DateTime? _lastUpdatedAt;
        private string _lastUpdatedBy;
        public DateTime CreatedAt => _createdAt;
        public string CreatedBy => _createdBy;
        public DateTime? LastUpdatedAt => _lastUpdatedAt;
        public string LastUpdatedBy => _lastUpdatedBy;
        public ICollection<Domain.Picture> Pictures { get; private set; }

        private Manufacturer()
        {
            Name = string.Empty;
            Description = string.Empty;
            _createdAt = DateTime.UtcNow;
            _createdBy = string.Empty;
            _lastUpdatedAt = default;
            _lastUpdatedBy = string.Empty;
            Pictures = new List<Domain.Picture>();
        }

        public Manufacturer(Guid id, string uniqueClientId, string name, string description) : this()
        {
            Id = id;
            UniqueClientId = uniqueClientId;
            Name = name;
            Description = description;
        }

        public void AddPicture(Guid id, Guid fileStorageUploadId, string filename, string description, string url, MimeType mimeType)
        {
            if (string.IsNullOrEmpty(filename))
                throw new DomainException($"{nameof(filename)} cannot be null or empty!");

            if (string.IsNullOrEmpty(description))
                throw new DomainException($"{nameof(description)} cannot be null or empty!");

            if (string.IsNullOrEmpty(url))
                throw new DomainException($"{nameof(url)} cannot be null or empty!");

            var picture = new Domain.Picture(id,
                AggregateId,
                AggregateTypeName,
                fileStorageUploadId,
                filename,
                description,
                url,
                mimeType);

            Pictures.Add(picture);
        }

        public void DeletePicture(Guid pictureId)
        {
            var picture = Pictures.FirstOrDefault(x => x.Id.Equals(pictureId));

            if (picture is null)
                throw new DomainException("Picture does not exist!");

            Pictures.Remove(picture);
        }

        public static Manufacturer GetDraftManufacturer() => new Manufacturer
        {
            Id = Guid.Parse("73d79ef5-3365-4877-b653-135632bb7a71"),
            Name = "DRAFT",
            Description = "Draft category, which purpose is to aggregate newly added products.",
            UniqueClientId = "DRAFT"
        };
    }
}