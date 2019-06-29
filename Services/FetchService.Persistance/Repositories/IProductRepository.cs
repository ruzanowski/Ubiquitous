using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.FetchService.Domain.Entities.Product;

namespace U.FetchService.Persistance.Repositories
{
    public interface IProductRepository : IDisposable
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string productId);
        Task InsertProductAsync(Product product);
        Task InsertProductsAsync(IEnumerable<Product> product);
        Task DeleteProductAsync(int productId);
        Task UpdateProductAsync(Product product);
        Task SaveAsync();
    }
}