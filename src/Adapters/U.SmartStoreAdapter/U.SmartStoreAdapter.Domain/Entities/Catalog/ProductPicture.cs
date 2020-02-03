using System.Runtime.Serialization;
using U.SmartStoreAdapter.Domain.Entities.Media;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
	[DataContract]
	public class ProductPicture : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
		[DataMember]
		public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
		[DataMember]
		public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
		[DataMember]
		public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }
    }

}
