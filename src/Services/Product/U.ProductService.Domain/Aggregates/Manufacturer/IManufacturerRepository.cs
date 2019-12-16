using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Domain.SeedWork;

// ReSharper disable CheckNamespace


namespace U.ProductService.Domain
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        Task<Manufacturer> AddAsync(Manufacturer manufacturer);

        void Update(Product product);

        Task<Manufacturer> GetAsync(Guid manufacturerId);
        Task<IList<Manufacturer>> GetManyAsync();
        Task<Manufacturer> GetUniqueClientIdAsync(string uniqueClientId);

        Task<bool> AnyAsync(Guid id);
    }
}