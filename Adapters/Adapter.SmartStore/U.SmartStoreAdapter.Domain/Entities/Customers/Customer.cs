using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    [DataContract]
	public class Customer : BaseEntity
    {
        private ICollection<CustomerContent> _customerContent;
        private ICollection<CustomerRole> _customerRoles;

		/// <summary>
		/// Ctor
		/// </summary>
        public Customer()
        {
            CustomerGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the customer Guid
        /// </summary>
        [DataMember]
        public Guid CustomerGuid { get; set; }

		/// <summary>
		/// Gets or sets the username
		/// </summary>
        [DataMember]
        public string Username { get; set; }

		/// <summary>
		/// Gets or sets the email
		/// </summary>
        [DataMember]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password
		/// </summary>
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the password format
		/// </summary>
        public int PasswordFormatId { get; set; }

		/// <summary>
		/// Gets or sets the password salt
		/// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        [DataMember]
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is tax exempt
        /// </summary>
        [DataMember]
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
		[DataMember]
		public int AffiliateId { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
		[DataMember]
		public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
		[DataMember]
		public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
		[DataMember]
		public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
		public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
		public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
		[DataMember]
		public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
		public DateTime LastActivityDateUtc { get; set; }

		/// <summary>
		/// For future use
		/// </summary>
		public string Salutation { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		public string FullName { get; set; }

		public string Company { get; set; }

		public string CustomerNumber { get; set; }

		public DateTime? BirthDate { get; set; }

		#region Navigation properties


        /// <summary>
        /// Gets or sets customer generated content
        /// </summary>
        public virtual ICollection<CustomerContent> CustomerContent
        {
			get => _customerContent ?? (_customerContent = new HashSet<CustomerContent>());
			protected set => _customerContent = value;
        }

		/// <summary>
		/// Gets or sets the customer roles
		/// </summary>
		[DataMember]
		public virtual ICollection<CustomerRole> CustomerRoles
        {
			get => _customerRoles ?? (_customerRoles = new HashSet<CustomerRole>());
			protected set => _customerRoles = value;
		}


		#endregion
    }
}