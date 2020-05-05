using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
	/// <summary>
	/// Represents a product manufacturer mapping
	/// </summary>
	[DataContract]
	public class ProductManufacturer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
		[DataMember]
		public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer identifier
        /// </summary>
		[DataMember]
		public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer
        /// </summary>
        public virtual Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the product
        /// </summary>
        public virtual Product Product { get; set; }
    }

}
