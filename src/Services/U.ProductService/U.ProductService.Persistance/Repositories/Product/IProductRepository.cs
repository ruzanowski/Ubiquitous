using System;
using System.Threading.Tasks;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Domain.SeedWork;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> AddAsync(Product product);

        void Update(Product product);

        Task<Product> GetAsync(Guid productId);

        Task<Guid?> GetAggregateIdByAbsoluteComparerAsync(string externalSourceName, string externalSourceId);
    }
}