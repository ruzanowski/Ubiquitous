using System;
using System.Threading.Tasks;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Manufacturer;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        Task<Manufacturer> AddAsync(Manufacturer manufacturer);
        void Update(Manufacturer product);
        Task<Manufacturer> GetAsync(Guid manufacturerId, bool @readonly);
        Task<bool> AnyAsync(Guid id);
        Task InvalidateCacheAsync(Guid manufacturerId);
    }
}