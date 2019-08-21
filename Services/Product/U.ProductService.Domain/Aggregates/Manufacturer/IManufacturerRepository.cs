using System;
using System.Threading.Tasks;
using U.ProductService.Domain.SeedWork;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        Task<Manufacturer> AddAsync(Manufacturer product);

        void Update(Product product);

        Task<Manufacturer> GetAsync(Guid productId);

        Task<bool> AnyAsync(Guid id);
    }
}