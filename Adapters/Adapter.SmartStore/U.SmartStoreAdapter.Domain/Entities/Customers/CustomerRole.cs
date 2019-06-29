using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Customers
{
	/// <summary>
	/// Represents a customer role
	/// </summary>
	[DataContract]
	public class CustomerRole : BaseEntity
    {
        

		/// <summary>
		/// Gets or sets the customer role name
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the customer role is marked as free shiping
		/// </summary>
		[DataMember]
		public bool FreeShipping { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the customer role is marked as tax exempt
		/// </summary>
		[DataMember]
		public bool TaxExempt { get; set; }

		/// <summary>
		/// Gets or sets the tax display type
		/// </summary>
		[DataMember]
		public int? TaxDisplayType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the customer role is active
		/// </summary>
		[DataMember]
		public bool Active { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the customer role is system
		/// </summary>
		[DataMember]
		public bool IsSystemRole { get; set; }

		/// <summary>
		/// Gets or sets the customer role system name
		/// </summary>
		[DataMember]
		public string SystemName { get; set; }


      
    }
}