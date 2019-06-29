using System.Collections.Generic;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
	/// <summary>
	/// Represents a specification attribute
	/// </summary>
	[DataContract]
	public class SpecificationAttribute : BaseEntity
	{
        private ICollection<SpecificationAttributeOption> _specificationAttributeOptions;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the specification attribute alias
		/// </summary>
		[DataMember]
		public string Alias { get; set; }

		/// <summary>
		/// Gets or sets the display order
		/// </summary>
		[DataMember]
		public int DisplayOrder { get; set; }

		/// <summary>
		/// Gets or sets whether the specification attribute will be shown on the product page.
		/// </summary>
		[DataMember]
		public bool ShowOnProductPage { get; set; }

		/// <summary>
		/// Gets or sets whether the specification attribute can be filtered. Only effective in accordance with MegaSearchPlus plugin.
		/// </summary>
		[DataMember]
		//todo	[Index]
		public bool AllowFiltering { get; set; }

        /// <summary>
        /// Specifies whether option names should be included in the search index. Only effective in accordance with MegaSearchPlus plugin.
        /// </summary>
        [DataMember]
        public bool IndexOptionNames { get; set; }

        /// <summary>
        /// Gets or sets the specification attribute options
        /// </summary>
        [DataMember]
		public virtual ICollection<SpecificationAttributeOption> SpecificationAttributeOptions
        {
			get => _specificationAttributeOptions ?? (_specificationAttributeOptions = new HashSet<SpecificationAttributeOption>());
			protected set => _specificationAttributeOptions = value;
        }
    }
}
