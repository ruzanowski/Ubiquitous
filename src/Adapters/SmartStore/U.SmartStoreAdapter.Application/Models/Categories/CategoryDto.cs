namespace U.SmartStoreAdapter.Application.Models.Categories
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
        public int? PictureId { get; set; }
    }
}
