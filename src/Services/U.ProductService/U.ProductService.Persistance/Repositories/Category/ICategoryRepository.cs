using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> AddAsync(Category category);

        void Update(Category category);

        Task<Category> GetAsync(Guid categoryId);

        Task<bool> AnyAsync(Guid id);
        Task<IList<Category>> GetManyAsync();

    }
}