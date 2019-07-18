using System.Threading.Tasks;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Aggregates.Product
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Product Aggregate

    public interface IProductRepository : IRepository<Product>
    {
        Product Add(Product product);
        
        void Update(Product product);

        Task<Product> GetAsync(int orderId);
    }
}
