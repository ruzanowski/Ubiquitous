using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Tax
{
	/// <summary>
	/// Represents a tax category
	/// </summary>
	[DataContract]
	public class TaxCategory : BaseEntity
    {
		/// <summary>
		/// Gets or sets the name
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the display order
		/// </summary>
		[DataMember]
		public int DisplayOrder { get; set; }
    }

}
