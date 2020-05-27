using System;
using U.ProductService.Domain.Aggregates.Picture;
using U.ProductService.Domain.SeedWork;
// ReSharper disable CheckNamespace

namespace U.ProductService.Domain
{

    /// <summary>
    /// Picture entity
    /// </summary>
    public class Picture : Entity
    {
        public Guid FileStorageUploadId { get; private set; }

        public Guid AggregateRootId { get; private set; }
        public string AggregateRootName { get; private set; }

        public string FileName { get; private set; }
        public string Description { get; private set; }
        public string Url { get; private set; }
        public MimeType MimeType { get; private set; }
        private Picture(){}

        public Picture(Guid id, Guid aggregateRootId, string aggregateRootName, Guid fileStorageUploadId, string fileName, string description, string url, MimeType mimeType) : this()
        {
            Id = id;
            AggregateRootId = aggregateRootId;
            AggregateRootName = aggregateRootName;
            FileStorageUploadId = fileStorageUploadId;
            FileName = fileName;
            Description = description;
            Url = url;
            MimeType = mimeType;
        }
    }
}