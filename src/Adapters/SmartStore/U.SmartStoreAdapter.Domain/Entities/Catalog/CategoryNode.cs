using System;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
	public interface ICategoryNode
	{
		int ParentCategoryId { get; }
		string Name { get; }
		string Alias { get; }
		int? PictureId { get; }
		bool Published { get; }
		int DisplayOrder { get; }
		DateTime UpdatedOnUtc { get; }
		string BadgeText { get; }
		int BadgeStyle { get; }
	}

	[Serializable]
	public class CategoryNode : ICategoryNode
	{
		public int Id { get; set; }
		public int ParentCategoryId { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public int? PictureId { get; set; }
		public bool Published { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime UpdatedOnUtc { get; set; }
		public string BadgeText { get; set; }
		public int BadgeStyle { get; set; }
		public bool SubjectToAcl { get; set; }
		public bool LimitedToStores { get; set; }
	}
}
