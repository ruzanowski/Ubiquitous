using System;
using System.Threading.Tasks;
using U.ProductService.Domain.SeedWork;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> AddAsync(Category category);

        void Update(Category product);

        Task<Category> GetAsync(Guid categoryId);

        Task<bool> AnyAsync(Guid id);
    }
}