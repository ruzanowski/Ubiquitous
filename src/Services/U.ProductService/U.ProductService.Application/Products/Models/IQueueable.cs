namespace U.ProductService.Application.Products.Models
{
    public interface IQueueable
    {
        QueuedJob QueuedJob { get; }
    }
}