namespace U.ProductService.Domain
{
    public interface IPublishable
    {
        bool IsPublished { get; }
        void Publish();
        void UnPublish();
        
    }
}