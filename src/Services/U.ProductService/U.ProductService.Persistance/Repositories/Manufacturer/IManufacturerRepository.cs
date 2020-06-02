using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Product;
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