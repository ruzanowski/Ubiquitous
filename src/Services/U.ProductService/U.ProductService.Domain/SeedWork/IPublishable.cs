namespace U.ProductService.Domain.SeedWork
{
    public interface IPublishable
    {
        bool IsPublished { get; }
        void Publish();
        void UnPublish();

    }
}