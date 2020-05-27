namespace U.ProductService.Domain.Common
{
    public interface IPublishable
    {
        bool IsPublished { get; }
        void Publish();
        void Unpublish();

    }
}