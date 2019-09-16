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
        Task<Manufacturer> AddAsync(Manufacturer product);

        void Update(Product product);

        Task<Manufacturer> GetAsync(Guid productId);
        
        Task<IList<Manufacturer>> GetAllAsync(Func<Manufacturer, bool> exp);

        Task<bool> AnyAsync(Guid id);
    }
}