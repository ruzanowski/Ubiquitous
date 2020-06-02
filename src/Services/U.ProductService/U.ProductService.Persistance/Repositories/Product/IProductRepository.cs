using System;
using System.Threading;
using System.Threading.Tasks;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;

// ReSharper disable CheckNamespace

namespace U.ProductService.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> AddAsync(Product product);
        void Update(Product product);
        Task<Product> GetAsync(Guid productId, bool asNoTracking, CancellationToken cancellationToken);
        Task<Guid?> GetIdByExternalTupleAsync(string externalSourceName, string externalSourceId);
        Task InvalidateCache(Guid productId);
    }
}