using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities
{  
    /// <summary>
    /// Base class for entities
    /// </summary>
    [DataContract]
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
	    public override int GetHashCode()
	    {
		    return Id;
	    }

	    /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
		[DataMember]
	 	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

	    public override bool Equals(object obj)
		{
			return Equals(obj as BaseEntity);
		}

		bool IEquatable<BaseEntity>.Equals(BaseEntity other)
		{
			return Equals(other);
		}

		private bool Equals(BaseEntity other)
		{
			if (other == null)
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return false;
		}
		public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
