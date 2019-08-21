using System;
using System.Threading.Tasks;
using U.ProductService.Domain.SeedWork;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> AddAsync(Product product);

        void Update(Product product);

        Task<Product> GetAsync(Guid productId);
        
        Task<Product> GetByAlternativeIdAsync(string alternateId);

        Task<bool> AnyAsync(Guid id);

        Task<bool> AnyAlternateIdAsync(string barCode);
    }
}