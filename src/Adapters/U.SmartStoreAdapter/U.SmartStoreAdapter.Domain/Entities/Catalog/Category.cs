using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
    /// <summary>
    /// Represents a category
    /// </summary>
    [DataContract]
	[DebuggerDisplay("{Id}: {Name} (Parent: {ParentCategoryId})")]
	public class Category : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent category identifier
        /// </summary>
        [DataMember]
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        [DataMember]
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        [DataMember]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        [DataMember]
        public DateTime UpdatedOnUtc { get; set; }
    }
}
