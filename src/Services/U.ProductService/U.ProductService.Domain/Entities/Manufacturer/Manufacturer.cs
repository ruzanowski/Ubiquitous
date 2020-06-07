using System;
using System.Collections.Generic;
using System.Linq;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Entities.Manufacturer
{
    /// <summary>
    /// Represents a manufacturer
    /// </summary>
    public class Manufacturer : Entity, IAggregateRoot, ITrackable
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
        public ICollection<ManufacturerPicture> Pictures { get; private set; }

        private Manufacturer()
        {
            Name = string.Empty;
            Description = string.Empty;
            _createdAt = DateTime.UtcNow;
            _createdBy = string.Empty;
            _lastUpdatedAt = default;
            _lastUpdatedBy = string.Empty;
            Pictures = new HashSet<ManufacturerPicture>();
        }

        public Manufacturer(Guid id, string uniqueClientId, string name, string description) : this()
        {
            Id = id;
            UniqueClientId = uniqueClientId;
            Name = name;
            Description = description;
        }

        public void AttachPicture(Guid pictureId)
        {
            Pictures.Add(new ManufacturerPicture
            {
                ManufacturerId = Id,
                PictureId = pictureId
            });
        }

        public void DetachPicture(Guid pictureId)
        {
            var picture = Pictures.FirstOrDefault(x => x.PictureId.Equals(pictureId));

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