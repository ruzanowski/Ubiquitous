using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface ICategoryRepository : IRepository<ProductCategory>
    {
        Task<ProductCategory> AddAsync(ProductCategory category);

        void Update(ProductCategory category);

        Task<ProductCategory> GetAsync(Guid categoryId);

        Task<bool> AnyAsync(Guid id);
        Task<IList<ProductCategory>> GetManyAsync();

    }
}