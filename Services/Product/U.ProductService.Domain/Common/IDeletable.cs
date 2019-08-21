namespace U.ProductService.Domain
{
    public interface IDeletable
    {
        bool IsDeleted { get; }
        void SetAsDeleted();
    }
}